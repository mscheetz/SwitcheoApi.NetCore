using RESTApiAccess;
using RESTApiAccess.Interface;
using DateTimeHelpers;
using Newtonsoft.Json;
using SwitcheoApi.NetCore.Core;
using SwitcheoApi.NetCore.Data.Interface;
using SwitcheoApi.NetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcheoApi.NetCore.Data
{
    public class SwitcheoRepository : ISwitcheoRepository
    {
        private string _baseUrl = "https://api.switcheo.network";
        private IRESTRepository _restRepo;
        private Blockchain _blockchain;
        private Helper _helper;
        private DateTimeHelper _dtHelper;
        private TransactionProcessor _txnProcessor;
        private Security _security;
        private NeoWallet _neoWallet;
        private string _contract_version;
        private string _contract_hash;
        private string[] _contract_hashes;
        private Dictionary<string, Token> _tokens = new Dictionary<string, Token>();
        private bool _systemTimetamp = false;

        #region Constructor/Destructor

        /// <summary>
        /// Constructor for non-auth apis
        /// </summary>
        public SwitcheoRepository()
        {
            LoadRepository("", false, "", Blockchain.neo);
        }

        /// <summary>
        /// Constructor for non-auth apis
        /// </summary>
        public SwitcheoRepository(Blockchain blockchain = Blockchain.neo)
        {
            LoadRepository("", false, "", blockchain);
        }

        /// <summary>
        /// Constructor for non-auth apis
        /// </summary>
        /// <param name="version">Contract version (default = "")</param>
        public SwitcheoRepository(string version = "", Blockchain blockchain = Blockchain.neo)
        {
            LoadRepository("", false, version, blockchain);
        }

        /// <summary>
        /// Constructor for all apis
        /// </summary>
        /// <param name="loginValue">Neo public address or private key</param>
        /// <param name="version">Contract version (default = "")</param>
        public SwitcheoRepository(string loginValue, string version = "", Blockchain blockchain = Blockchain.neo)
        {
            LoadRepository(loginValue, false, version, blockchain);
        }

        /// <summary>
        /// Constructor for non-auth apis
        /// </summary>
        /// <param name="testRegion">Use test region (default = false)</param>
        /// <param name="version">Contract version (default = "")</param>
        public SwitcheoRepository(bool testRegion = false, string version = "", Blockchain blockchain = Blockchain.neo)
        {
            LoadRepository("", testRegion, version, blockchain);
        }

        /// <summary>
        /// Constructor for all apis
        /// </summary>
        /// <param name="loginValue">Neo public address or private key</param>
        /// <param name="testRegion">Use test region (default = false)</param>
        /// <param name="version">Contract version (default = "")</param>
        public SwitcheoRepository(string loginValue, bool testRegion = false, string version = "", Blockchain blockchain = Blockchain.neo)
        {
            LoadRepository(loginValue, testRegion, version, blockchain);
        }

        #endregion Constructor/Destructor

        private void LoadRepository(string addyPk, bool testRegion, string version, Blockchain blockchain)
        {
            _helper = new Helper();
            if (string.IsNullOrEmpty(addyPk))
            {
                _neoWallet = null;
            }
            else if (addyPk.First() == 'A' || addyPk.Substring(0,2) == "0x")
            {
                _neoWallet = new NeoWallet(addyPk);
            }
            else
            {
                var pkSecure = _helper.GetSecureString(addyPk);
                _neoWallet = new NeoWallet(pkSecure);
            }
            var testSwitch = testRegion ? "test-" : string.Empty;
            _baseUrl = $"https://{testSwitch}api.switcheo.network";
            _restRepo = new RESTRepository();
            _security = new Security();
            _txnProcessor = new TransactionProcessor();
            _tokens = GetTokens();
            _blockchain = blockchain;
            _contract_version = version;
            _contract_hash = GetContractHash();
            _dtHelper = new DateTimeHelper();
            _systemTimetamp = TimestampCompare();
        }

        /// <summary>
        /// Compare exchange and system unix timestamps
        /// </summary>
        /// <returns>True if difference less than 1000 MS, otherwise false</returns>
        private bool TimestampCompare()
        {
            var exchangeTS = GetTimestamp();
            var systemTS = _dtHelper.UTCtoUnixTimeMilliseconds();
            if(exchangeTS == systemTS || Math.Abs((decimal)exchangeTS - (decimal)systemTS) < 1000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get server timestamp
        /// </summary>
        /// <returns>Long of timestamp</returns>
        public async Task<long> GetServerTime()
        {
            var endpoint = "/v2/exchange/timestamp";
            var url = _baseUrl + endpoint;

            var response = await _restRepo.GetApi<Dictionary<string, long>>(url);

            return response["timestamp"];
        }

        /// <summary>
        /// Get hashes of contracts deployed by Switcheo
        /// </summary>
        /// <returns>Contracts dictionary</returns>
        public async Task<Dictionary<string, Dictionary<string, string>>> GetContracts()
        {
            var endpoint = "/v2/exchange/contracts";

            var url = _baseUrl + endpoint;

            try
            {
                var response = await _restRepo.GetApiStream<Dictionary<string, Dictionary<string, string>>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieve a list of supported tokens on Switcheo.
        /// </summary>
        /// <returns>Tokens dictionary</returns>
        public Dictionary<string, Token> GetTokens()
        {
            if (_tokens.Any())
                return _tokens;
            else
                return OnGetTokens().Result;
        }

        /// <summary>
        /// Retrieve a list of supported tokens on Switcheo.
        /// </summary>
        /// <returns>Tokens dictionary</returns>
        private async Task<Dictionary<string, Token>> OnGetTokens()
        {
            var endpoint = "/v2/exchange/tokens";

            var url = _baseUrl + endpoint;

            try
            {
                var response = await _restRepo.GetApiStream<Dictionary<string, Token>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get a Token by symbol
        /// </summary>
        /// <param name="symbol">Symbol of token</param>
        /// <returns>Token object</returns>
        public Token GetTokenBySymbol(string symbol)
        {
            return !string.IsNullOrEmpty(symbol) ? _tokens[symbol] : null;
        }
        
        /// <summary>
        /// Get a token by hash
        /// </summary>
        /// <param name="hash">Hash of token</param>
        /// <returns>Token symbol value</returns>
        public string GetTokenByHash(string hash)
        {
            return !string.IsNullOrEmpty(hash) ? _tokens.FirstOrDefault(t => t.Value.hash.Equals(hash)).Key : null;
        }

        /// <summary>
        /// Get available currency pairs on Switcheo Exchange 
        /// </summary>
        /// <param name="bases">Base pairs to filter on (default all pairs)</param>
        /// <returns>Array of trading pairs</returns>
        public async Task<string[]> GetPairs(string[] bases = null)
        {
            var endpoint = "/v2/exchange/pairs";

            var querystring = string.Empty;
            if (bases != null)
            {
                for (var i = 0; i < bases.Length; i++)
                {
                    querystring += querystring == string.Empty ? "?" : "&";

                    querystring += $"bases[]={bases[i]}";
                }
            }

            var url = _baseUrl + endpoint + querystring;
            
            try
            {
                var response = await _restRepo.GetApiStream<string[]>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Get candlestick chart data
        /// </summary>
        /// <param name="pair">Pair to filter</param>
        /// <param name="interval">Time interval of candlestick</param>
        /// <param name="endTime">End time of last candlestick (defaults to current UTC time)</param>
        /// <param name="stickCount">Number of candlesticks to return (defaults to 10)</param>
        /// <returns>Array of Candlesticks</returns>
        public async Task<Candlstick[]> GetCandlesticks(string pair, Interval interval, long endTime = 0, int stickCount = 10)
        {
            if(endTime == 0)
            {
                endTime = _dtHelper.UTCtoUnixTime();
            }
            var sticks = stickCount < 10 ? 10 : stickCount;
            var startTime = _helper.GetFromUnixTime(endTime, interval, sticks);

            var candlesticks = await GetCandleSticks(pair, interval, endTime, startTime);

            if (candlesticks.Length > stickCount)
            {
                candlesticks = candlesticks.Take(stickCount).ToArray();
            }

            return candlesticks;
        }

        /// <summary>
        /// Get candlestick chart data
        /// </summary>
        /// <param name="pair">Pair to filter</param>
        /// <param name="interval">Time interval of candlestick</param>
        /// <param name="endTime">Unix time of last candlestick</param>
        /// <param name="startTime">Unix time of 1st candlestick</param>
        /// <returns>Array of Candlesticks</returns>
        public async Task<Candlstick[]> GetCandleSticks(string pair, Interval interval, long endTime, long startTime)
        {
            var endpoint = "/v2/tickers/candlesticks";
            var queryString = new List<string>
            {
                $"end_time={endTime}",
                $"interval={(int)interval}",
                $"pair={pair}",
                $"start_time={startTime}"
            };

            var url = _baseUrl + endpoint + "?" + _helper.ListToQueryString(queryString);

            try
            {
                var response = await _restRepo.GetApiStream<Candlstick[]>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get 24-hour data for all pairs and markets
        /// </summary>
        /// <returns>Array of Candlesticks</returns>
        public async Task<Candlstick[]> GetLast24Hours()
        {
            var endpoint = "/v2/tickers/last_24_hours";
            var url = _baseUrl + endpoint;

            try
            {
                var response = await _restRepo.GetApiStream<Candlstick[]>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get last price of given symbol
        /// </summary>
        /// <param name="symbols">String array of currency symbols (default null)</param>
        /// <param name="bases">String array of base pairs (default null)</param>
        /// <returns>LastPrices dictionary</returns>
        public async Task<Dictionary<string, Dictionary<string, decimal>>> GetLastPrice(string[] symbols = null, string[] bases = null)
        {
            var endpoint = "/v2/tickers/last_price";
            var querystring = string.Empty;

            if(symbols != null || bases != null)
            {
                if (symbols != null)
                {
                    for (var i = 0; i < symbols.Length; i++)
                    {
                        querystring += querystring == string.Empty ? "?" : "&";

                        querystring += $"symbols[]={symbols[i]}";
                    }
                }
                if (bases != null)
                {
                    for (var i = 0; i < bases.Length; i++)
                    {
                        querystring += querystring == string.Empty ? "?" : "&";

                        querystring += $"bases[]={bases[i]}";
                    }
                }
            }

            var url = _baseUrl + endpoint + querystring;

            try
            {
                var response = await _restRepo.GetApiStream<Dictionary<string, Dictionary<string, decimal>>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get last price for a trading pair
        /// </summary>
        /// <param name="pair">String of trading pair</param>
        /// <returns>Decimal of last price</returns>
        public async Task<decimal> GetLastPrice(string pair)
        {
            var asset = pair.Substring(0, pair.IndexOf("_"));
            var baseSymbol = pair.Substring(pair.IndexOf("_") + 1);
            var prices = await GetLastPrice();

            var assetList = prices[asset];

            return assetList[baseSymbol];
        }

        /// <summary>
        /// Get best 70 offers on the offer book with converted values
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>OrderBook object</returns>
        public async Task<OrderBook> GetOrderBook(string pair)
        {
            var offers = await GetOffers(pair);

            var offerList = offers.ToList();
            var orderBookList = offerList.Select(o => new SwitcheoOffer
            {
                available_amount = _helper.DeCalculateAmount(pair, o.available_amount, _tokens),
                id = o.id,
                offer_amount = _helper.DeCalculateAmount(pair, o.offer_amount, _tokens),
                offer_asset = o.offer_asset,
                want_amount = _helper.DeCalculateAmount(pair, o.want_amount, _tokens),
                want_asset = o.want_asset
            }).ToList();

            var orderBook = new OrderBook
            {
                asks = orderBookList.Where(o => o.orderType == OrderBookType.ask).OrderBy(o => o.price).ToArray(),
                bids = orderBookList.Where(o => o.orderType == OrderBookType.bid).OrderByDescending(o => o.price).ToArray()
            };

            return orderBook;
        }

        /// <summary>
        /// Get best 70 offers on the offer book
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>Array of Offers</returns>
        public async Task<Offer[]> GetOffers(string pair)
        {
            var endpoint = "/v2/offers";

            var querystring = new List<string>
            {
                $"blockchain=neo",
                $"contract_hash={_contract_hash}",
                $"pair={pair}"
            };

            var url = _baseUrl + endpoint + "?" + _helper.ListToQueryString(querystring);

            try
            {
                var response = await _restRepo.GetApiStream<Offer[]>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>Array of TradeDetail</returns>
        public async Task<TradeDetail[]> GetTrades(string pair)
        {
            return await OnGetTrades(pair);
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="tradeCount">Number of trades to return</param>
        /// <returns>Array of TradeDetail</returns>
        public async Task<TradeDetail[]> GetTrades(string pair, int tradeCount)
        {
            return await OnGetTrades(pair, null, null, tradeCount);
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="fromDate">Only return trades after this date (default = null)</param>
        /// <param name="toDate">Only return trades before this date (default = null)</param>
        /// <returns>Array of TradeDetail</returns>
        public async Task<TradeDetail[]> GetTrades(string pair, DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null)
        {
            return await OnGetTrades(pair, fromDate, toDate);
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="fromDate">Only return trades after this date (default = null)</param>
        /// <param name="toDate">Only return trades before this date (default = null)</param>
        /// <param name="tradeCount">Number of trades to return</param>
        /// <returns>Array of TradeDetail</returns>
        public async Task<TradeDetail[]> GetTrades(string pair, DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null, int tradeCount = 5000)
        {
            return await OnGetTrades(pair, fromDate, toDate, tradeCount);
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="fromDate">Only return trades after this date (default = null)</param>
        /// <param name="toDate">Only return trades before this date (default = null)</param>
        /// <param name="tradeCount">Number of trades to return (default = 5000)</param>
        /// <returns>Array of TradeDetail</returns>
        private async Task<TradeDetail[]> OnGetTrades(string pair, DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null, int tradeCount = 5000)
        {
            var endpoint = "/v2/trades";
            tradeCount = 
                tradeCount == 0 ? 1 
                : tradeCount > 10000 ? 10000 
                : tradeCount;

            var querystring = new List<string>
            {
                $"blockchain=neo",
                $"contract_hash={_contract_hash}",
                $"pair={pair}"
            };

            if (tradeCount != 5000)
            {
                querystring.Add($"limit={tradeCount}");
            }

            if (fromDate != null && toDate != null)
            {
                if (fromDate > toDate)
                {
                    throw new Exception("From date cannot be after to date");
                }
            }

            if(fromDate != null)
            {
                var from = _dtHelper.UTCtoUnixTime((DateTimeOffset)fromDate);
                querystring.Add($"from={from}");
            }
            if (toDate != null)
            {
                var to = _dtHelper.UTCtoUnixTime((DateTimeOffset)toDate);
                querystring.Add($"to={to}");
            }

            var url = _baseUrl + endpoint + "?" + _helper.ListToQueryString(querystring);

            try
            {
                var response = await _restRepo.GetApiStream<TradeDetail[]>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get loaded wallet
        /// </summary>
        /// <returns>NeoWallet if loaded</returns>
        public NeoWallet GetWallet()
        {
            if (_neoWallet != null)
            {
                return _neoWallet;
            }
            else
            {
                throw new Exception("Wallet not loaded! Load project with a public key, address, or private key (WIF).");
            }
        }

        /// <summary>
        /// Get contract balance for signed in user
        /// </summary>
        /// <returns>Balance response</returns>
        public async Task<BalanceResponse> GetBalances()
        {
            return await OnGetBalances(_neoWallet.exchangeAddress);
        }

        /// <summary>
        /// Get contract balance of a given address
        /// </summary>
        /// <param name="scriptHash">String of script hash (address)</param>
        /// <returns>Balance response</returns>
        public async Task<BalanceResponse> GetBalances(string scriptHash)
        {
            return await OnGetBalances(scriptHash);
        }

        /// <summary>
        /// Get contract balance of a given address
        /// </summary>
        /// <param name="address">String of addresses</param>
        /// <returns>Balance response</returns>
        private async Task<BalanceResponse> OnGetBalances(string address)
        {
            var endpoint = "/v2/balances";

            var hashQueryString = string.Empty;
            for (int i = 0; i < _contract_hashes.Length; i++)
            {
                hashQueryString += $"&contract_hashes[]={_contract_hashes[i]}";
            }

            var url = _baseUrl + endpoint + $"?addresses[]={address}" + hashQueryString;

            try
            {
                var response = await _restRepo.GetApiStream<BalanceResponse>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Create and execute a deposit to the exchange
        /// </summary>
        /// <param name="asset">Asset to deposit</param>
        /// <param name="amount">Amount to deposit</param>
        /// <returns>Boolean when complete</returns>
        public async Task<bool> Deposit(string asset, decimal amount)
        {
            var deposit = await CreateDeposit(asset, amount);

            var execution = await ExecuteDeposit(deposit);

            return execution == null ? false : true;
        }

        /// <summary>
        /// Create a deposit
        /// </summary>
        /// <param name="asset">Asset to deposit</param>
        /// <param name="amount">Amount to deposit</param>
        /// <returns>Deposit response</returns>
        public async Task<TransactionResponse> CreateDeposit(string asset, decimal amount)
        {
            var endpoint = "/v2/deposits";
            var timestamp = GetTimestamp();

            var param = new Dictionary<string, object>();

            param.Add("amount", GetAmount(asset, amount));
            param.Add("asset_id", asset);
            param.Add("blockchain", _blockchain.ToString());
            param.Add("contract_hash", _contract_hash);
            param.Add("timestamp", timestamp);

            var signature = SignMessage(param);

            param.Add("address", _neoWallet.exchangeAddress);
            param.Add("signature", signature);
            
            var url = _baseUrl + endpoint;

            try
            {
                var response = await _restRepo.PostApi<TransactionResponse, Dictionary<string, object>>(url, param);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Execute a deposit
        /// </summary>
        /// <param name="deposit">Deposit detail from creation</param>
        /// <returns>Deposit response</returns>
        public async Task<TransactionResponse> ExecuteDeposit(TransactionResponse deposit)
        {
            var sigDic = new Dictionary<string, object>();
            sigDic.Add("signature", SignTransaction(deposit.transaction));

            var endpoint = "/v2/deposits";
            
            var url = _baseUrl + endpoint + $"/{deposit.id}/broadcast";

            try
            {
                var response = await _restRepo.PostApi<TransactionResponse, Dictionary<string, object>>(url, sigDic);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Create and execute a withdrawal from the exchange
        /// </summary>
        /// <param name="asset">Asset to withdrawal</param>
        /// <param name="amount">Amount to withdrawal</param>
        /// <returns>Boolean when complete</returns>
        public async Task<bool> Withdrawal(string asset, decimal amount)
        {
            var withdrawal = await CreateWithdrawal(asset, amount);

            var execution = await ExecuteWithdrawal(withdrawal);

            return execution == null ? false : true;
        }

        /// <summary>
        /// Create a withdrawal
        /// </summary>
        /// <param name="asset">Asset to withdrawal</param>
        /// <param name="amount">Amount to withdrawal</param>
        /// <returns>Withdrawal id</returns>
        public async Task<string> CreateWithdrawal(string asset, decimal amount)
        {
            var endpoint = "/v2/withdrawals";
            var timestamp = GetTimestamp();

            var param = new Dictionary<string, object>();

            param.Add("amount", GetAmount(asset, amount));
            param.Add("asset_id", asset);
            param.Add("blockchain", _blockchain.ToString());
            param.Add("contract_hash", _contract_hash);
            param.Add("timestamp", timestamp);

            var signature = SignMessage(param);

            param.Add("address", _neoWallet.exchangeAddress);
            param.Add("signature", signature);

            var url = _baseUrl + endpoint;

            try
            {
                var response = await _restRepo.PostApi<WithdrawalId, Dictionary<string, object>>(url, param);
                
                return response.id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Execute a withdrawal
        /// </summary>
        /// <param name="withdrawalId">Guid of withdrawal request</param>
        /// <returns>Withdrawal response</returns>
        public async Task<WithdrawalResponse> ExecuteWithdrawal(string withdrawalId)
        {
            var timestamp = GetTimestamp();

            var sigDic = new Dictionary<string, object>();
            sigDic.Add("id", withdrawalId.ToString());
            sigDic.Add("timestamp", timestamp);

            var signature = SignMessage(sigDic);

            sigDic.Add("signature", signature);

            var endpoint = "/v2/withdrawals";

            var url = _baseUrl + endpoint + $"/{withdrawalId}/broadcast";

            try
            {
                var response = await _restRepo.PostApi<WithdrawalResponse, Dictionary<string, object>>(url, sigDic);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        /// Get an Order by Id
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Order object</returns>
        public async Task<Order> GetOrder(string id)
        {
            var address = _neoWallet.exchangeAddress;

            var orders = await OnGetOrders(address);

            return orders.Where(o => o.id.Equals(id)).FirstOrDefault();
        }

        /// <summary>
        /// Get all open Orders
        /// </summary>
        /// <returns>Array of Order objects</returns>
        public async Task<Order[]> GetOpenOrders()
        {
            var address = _neoWallet.exchangeAddress;

            var orders = await OnGetOrders(address);

            var openOrders = orders.Where(o => !string.IsNullOrEmpty(o.order_status) 
                                                && o.order_status.Equals("open")).ToList();

            return openOrders.ToArray();
        }

        /// <summary>
        /// Get all completed Orders
        /// </summary>
        /// <returns>Array of Order objects</returns>
        public async Task<Order[]> GetCompletedOrders()
        {
            var address = _neoWallet.exchangeAddress;

            var orders = await OnGetOrders(address);

            var openOrders = orders.Where(o => !string.IsNullOrEmpty(o.order_status) 
                                                && o.order_status.Equals("completed")).ToList();

            return openOrders.ToArray();
        }

        /// <summary>
        /// Get an order by order id
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>Order object</returns>
        public async Task<Order> GetOrder(string id)
        {
            var address = _neoWallet.address;

            var orders = await OnGetOrders(address);

            return orders.Where(o => o.id.Equals(id)).FirstOrDefault();
        }

        /// <summary>
        /// Get orders for current address
        /// </summary>
        /// <returns>Array of orders</returns>
        public async Task<Order[]> GetOrders()
        {
            var address = _neoWallet.exchangeAddress;

            return await OnGetOrders(address);
        }

        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <returns>Array of orders</returns>
        public async Task<Order[]> GetOrders(string address)
        {
            return await OnGetOrders(address);
        }

        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <param name="pair">String of pair to match</param>
        /// <returns>Array of orders</returns>
        public async Task<Order[]> GetOrders(string address, string pair)
        {
            return await OnGetOrders(address, pair);
        }

        private async Task<SwitcheoOrder[]> OnGetSwitcheoOrders(string address, string pair = "")
        {
            var orders = await OnGetOrders(address, pair);
            
            var orderList = orders.ToList();
            var switcheoOrderList = orderList.Select(o => new SwitcheoOrder
            {
                address = o.address,
                avgFillPrice = o.fills.Sum(f => decimal.Parse(f.price)),
                createdAt = o.created_at,
                blockchain = o.blockchain,
                feeAmount = o.fills.Sum(f => decimal.Parse(f.fee_amount)),
                feeAsset = GetTokenByHash(o.fills.Select(f => f.fee_asset_id).FirstOrDefault()),
                filledQuanity = o.fills.Sum(f => decimal.Parse(f.filled_amount)),
                offerAsset = GetTokenByHash(o.offer_asset_id),
                orderStatus = o.order_status,
                originalQuantity = decimal.Parse(o.offer_amount),
                offerAmount = _helper.DeCalculateAmount(decimal.Parse(o.offer_amount)),
                remainingQuantity = o.makes.Sum(m => decimal.Parse(m.offer_amount)),
                side = o.side,
                status = o.status,
                useSWTH = o.use_native_token,
                wantAmount = _helper.DeCalculateAmount(decimal.Parse(o.want_amount)),
                wantAsset = GetTokenByHash(o.want_asset_id),
                id = o.id,
            }).ToList();

            return switcheoOrderList.ToArray();
        }

        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <param name="pair">String of pair to match</param>
        /// <returns>Array of orders</returns>
        private async Task<Order[]> OnGetOrders(string address, string pair = "")
        {
            var endpoint = "/v2/orders";

            var queryString = $"?address={address}&contract_hash={_contract_hash}";
            if (pair != "")
            {
                queryString += $"&pair={pair}";
            }

            queryString += $"&contract_hash={_contract_hash}";

            var url = _baseUrl + endpoint + queryString;

            try
            {
                var response = await _restRepo.GetApiStream<Order[]>(url);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This endpoint creates an order which can be executed through BroadcastOrder.
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="neoAmount">Decimal amount of NEO to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        public async Task<Order> CreateNeoOrder(string pair, Side side, decimal price, decimal neoAmount, bool useSWTH = true)
        {
            return await OnCreateOrder(pair, side, price, neoAmount, useSWTH);
        }

        /// <summary>
        /// This endpoint creates an order which can be executed through BroadcastOrder.
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="tokenAmount">Decimal amount of tokens to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        public async Task<Order> CreateTokenOrder(string pair, Side side, decimal price, decimal tokenAmount, bool useSWTH = true)
        {
            var wantAmount = price * tokenAmount;

            return await OnCreateOrder(pair, side, price, wantAmount, useSWTH);
        }

        /// <summary>
        /// This endpoint places an order using Neo amount
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="neoAmount">Decimal amount of NEO to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        public async Task<Order> PlaceNeoOrder(string pair, Side side, decimal price, decimal neoAmount, bool useSWTH = true)
        {
            return await OnCreateAndExecuteOrder(pair, side, price, neoAmount, useSWTH);
        }

        /// <summary>
        /// This endpoint places an order using token amount
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="amount">Decimal amount of tokens to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        public async Task<Order> PlaceOrder(string pair, Side side, decimal price, decimal amount, bool useSWTH = true)
        {
            var wantAmount = price * amount;
            
            return await OnCreateAndExecuteOrder(pair, side, price, wantAmount, useSWTH);
        }

        /// <summary>
        /// This endpoint creates and broadcasts an order.
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="amount">Decimal of order amount</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        private async Task<Order> OnCreateAndExecuteOrder(string pair, Side side, decimal price, decimal amount, bool useSWTH = true)
        {
            var order = await OnCreateOrder(pair, side, price, amount, useSWTH);

            var broadcasted = await BroadcastOrder(order);

            return broadcasted;
        }

        /// <summary>
        /// This endpoint creates an order which can be executed through BroadcastOrder.
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="amount">Decimal of order amount</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        private async Task<Order> OnCreateOrder(string pair, Side side, decimal price, decimal amount, bool useSWTH = true)
        {
            var endpoint = "/v2/orders";
            var url = _baseUrl + endpoint;

            var timestamp = GetTimestamp();
            
            var request = new Dictionary<string, object>();
            request.Add("blockchain", _blockchain.ToString());
            request.Add("contract_hash", _contract_hash.ToLower());
            request.Add("order_type", "limit");
            request.Add("pair", pair.ToUpper());
            request.Add("price", _helper.DecimalToString(price));
            request.Add("side", side.ToString().ToLower());
            request.Add("timestamp", timestamp);
            request.Add("use_native_tokens", useSWTH);
            request.Add("want_amount", GetAmount(pair, amount));

            var signature = SignMessage(request);

            request.Add("address", _neoWallet.exchangeAddress);
            request.Add("signature", signature);

            try
            {
                var response = await _restRepo.PostApi<Order, Dictionary<string, object>>(url, request);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This is the second endpoint required to execute an order. 
        /// After using the CreateOrder endpoint, 
        /// you will receive a response which needs to be signed.
        /// </summary>
        /// <param name="order">Order created</param>
        /// <returns>Boolean when complete</returns>
        public async Task<Order> BroadcastOrder(Order order)
        {
            var endpoint = $"/v2/orders/{order.id}/broadcast";
            var url = _baseUrl + endpoint;

            var signatures = new Dictionary<string, object>();
            signatures.Add("fills", SignFills(order.fills));
            signatures.Add("makes", SignMakes(order.makes));

            var request = new Dictionary<string, object>();
            request.Add("signatures", signatures);

            try
            {
                var response = await _restRepo.PostApi<Order, Dictionary<string, object>>(url, request);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="orderId">Id of order to cancel</param>
        /// <returns>Cancelled Order object</returns>
        public async Task<Order> CancelOrder(string orderId)
        {
            var cancellationRequest = await CreateCancellation(orderId);

            var cancelledOrder = await ExecuteCancellation(cancellationRequest);

            return cancelledOrder;
        }

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="order">Order to cancel</param>
        /// <returns>Cancelled Order object</returns>
        public async Task<Order> CancelOrder(Order order)
        {
            var cancellationRequest = await CreateCancellation(order);

            var cancelledOrder = await ExecuteCancellation(cancellationRequest);

            return cancelledOrder;
        }

        /// <summary>
        /// This is the first API call required to cancel an order. 
        /// Only orders with makes and with 
        /// an available_amount of more than 0 can be cancelled.
        /// </summary>
        /// <param name="orderId">Order Id to be cancelled</param>
        /// <returns>TransactionResponse when complete</returns>
        public async Task<TransactionResponse> CreateCancellation(string orderId)
        {
            var order = new Order
            {
                id = orderId
            };

            return await OnCreateCancellation(order);
        }

        /// <summary>
        /// This is the first API call required to cancel an order. 
        /// Only orders with makes and with 
        /// an available_amount of more than 0 can be cancelled.
        /// </summary>
        /// <param name="order">Order to be cancelled</param>
        /// <returns>TransactionResponse when complete</returns>
        public async Task<TransactionResponse> CreateCancellation(Order order)
        {
            return await OnCreateCancellation(order);
        }

        /// <summary>
        /// This is the first API call required to cancel an order. 
        /// Only orders with makes and with 
        /// an available_amount of more than 0 can be cancelled.
        /// </summary>
        /// <param name="order">Order to be cancelled</param>
        /// <returns>TransactionResponse when complete</returns>
        public async Task<TransactionResponse> OnCreateCancellation(Order order)
        {
            var endpoint = $"/v2/cancellations";
            var url = _baseUrl + endpoint;
            var timestamp = GetTimestamp();

            var sigDic = new Dictionary<string, object>();
            sigDic.Add("order_id", order.id.ToString());
            sigDic.Add("timestamp", timestamp);

            var signature = SignMessage(sigDic);

            sigDic.Add("signature", signature);
            sigDic.Add("address", _neoWallet.exchangeAddress);

            try
            {
                var response = await _restRepo.PostApi<TransactionResponse, Dictionary<string, object>>(url, sigDic);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This is the second endpoint that must be called to cancel an order. 
        /// After calling the CreateCancellation endpoint, 
        /// you will receive a transaction in the response which must be signed.
        /// </summary>
        /// <param name="cancellation">Cancellation object</param>
        /// <returns>Order object</returns>
        public async Task<Order> ExecuteCancellation(TransactionResponse cancellation)
        {
            var endpoint = $"/v2/cancellations/{cancellation.id}/broadcast";
            var url = _baseUrl + endpoint;

            var sigDic = new Dictionary<string, object>();
            sigDic.Add("signature", SignTransaction(cancellation.transaction));

            try
            {
                var response = await _restRepo.PostApi<Order, Dictionary<string, object>>(url, sigDic);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Sign a message
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="toSign">Message to sign</param>
        /// <returns>String of signature</returns>
        private string SignMessage<T>(T toSign)
        {
            var messageString = JsonConvert.SerializeObject(toSign);
            return SignMessage(messageString);
        }

        /// <summary>
        /// Sign a message
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <returns>String of signature</returns>
        private string SignMessage(string message)
        {
            return _security.SignMessage(message, _neoWallet);
        }

        /// <summary>
        /// Get converted exchange amount
        /// </summary>
        /// <param name="asset">Asset to calculate</param>
        /// <param name="amount">Amount of token to convert</param>
        /// <returns>String of exchange amount</returns>
        private string GetAmount(string asset, decimal amount)
        {
            return _helper.CalculateAmount(asset, amount, _tokens);
        }

        /// <summary>
        /// Get unix timestamp
        /// </summary>
        /// <returns>Long of timestamp</returns>
        private long GetTimestamp()
        {
            if (_systemTimetamp)
            {
                return _dtHelper.UTCtoUnixTimeMilliseconds();
            }
            else
            {
                return GetServerTime().Result;
            }
        }

        /// <summary>
        /// Sign fills in an order
        /// </summary>
        /// <param name="fills">Array of fills to sign</param>
        /// <returns>String array of signatures</returns>
        private Dictionary<string, string> SignFills(Fill[] fills)
        {
            var sigList = new Dictionary<string, string>();

            for (int i = 0; i < fills.Length; i++)
            {
                var signature = SignTransaction(fills[i].txn);

                sigList.Add(fills[i].id, signature);
            }

            return sigList;
        }

        /// <summary>
        /// Sign makes in an order
        /// </summary>
        /// <param name="makes">Array of makes to sign</param>
        /// <returns>String array of signatures</returns>
        private Dictionary<string, string> SignMakes(Make[] makes)
        {
            var txnProcessor = new TransactionProcessor();
            var sigList = new Dictionary<string, string>();

            for (int i = 0; i < makes.Length; i++)
            {
                var signature = SignTransaction(makes[i].txn);

                sigList.Add(makes[i].id, signature);
            }

            return sigList;
        }

        /// <summary>
        /// Sign a Transaction object
        /// </summary>
        /// <param name="txn">Transaction to sign</param>
        /// <returns>String of signature</returns>
        private string SignTransaction(Transaction txn)
        {
            var signedTxn = _txnProcessor.GetSignedTransaction(txn);
            var serializedTxn = signedTxn.Serialize(false);
            var signature = _security.SignTransaction(serializedTxn, _neoWallet);

            return signature;
        }

        /// <summary>
        /// Gets latest or specified contract hash
        /// </summary>
        /// <returns>String of hash</returns>
        private string GetContractHash()
        {
            var contracts = GetContracts().Result;

            var versions = contracts[_blockchain.ToString().ToUpper()];

            var hash = string.Empty;

            if(_contract_version == "")
            {
                hash = versions.Values.Last();
            }
            else
            {
                hash = versions[_contract_version];
            }

            _contract_hashes = versions.Values.ToArray();

            return hash;
        }
    }
}
