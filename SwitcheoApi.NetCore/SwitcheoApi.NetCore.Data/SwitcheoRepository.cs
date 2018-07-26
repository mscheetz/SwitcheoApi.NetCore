using SwitcheoApi.NetCore.Core;
using SwitcheoApi.NetCore.Core.Interface;
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
        private IRestRepository _restRepo;
        private Helper _helper;
        private Security _security;
        private string _contract_version;
        private string _contract_hash;
        private string[] _contract_hashes;
        private string _address;
        private string _key;

        #region Constructor/Destructor

        /// <summary>
        /// Constructor for non-auth apis
        /// </summary>
        /// <param name="version">Contract version (default = "")</param>
        public SwitcheoRepository(string version = "")
        {
            _restRepo = new RestRepository();
            _helper = new Helper();
            _security = new Security();
            _contract_version = version;
            _contract_hash = GetContractHash();
        }

        /// <summary>
        /// Constructor for all apis
        /// </summary>
        /// <param name="address">Neo wallet address</param>
        /// <param name="privateKey">Neo wallet primary key</param>
        /// <param name="version">Contract version (default = "")</param>
        public SwitcheoRepository(string address, string privateKey, string version = "")
        {
            _address = address;
            _key = privateKey;
            _restRepo = new RestRepository();
            _helper = new Helper();
            _security = new Security();
            _contract_version = version;
            _contract_hash = GetContractHash();
        }

        /// <summary>
        /// Constructor for test region non-auth apis
        /// </summary>
        /// <param name="version">Contract version (default = "")</param>
        public SwitcheoRepository(bool testRegion = false, string version = "")
        {
            _baseUrl = "https://test-api.switcheo.network";
            _restRepo = new RestRepository();
            _helper = new Helper();
            _security = new Security();
            _contract_version = version;
            _contract_hash = GetContractHash();
        }

        /// <summary>
        /// Constructor for test region for all apis
        /// </summary>
        /// <param name="address">Neo wallet address</param>
        /// <param name="privateKey">Neo wallet primary key</param>
        /// <param name="testRegion">Boolean to use test region</param>
        /// <param name="version">Contract version (default = "")</param>
        public SwitcheoRepository(string address, string privateKey, bool testRegion = false, string version = "")
        {
            _address = address;
            _key = privateKey;
            _baseUrl = "https://test-api.switcheo.network";
            _restRepo = new RestRepository();
            _helper = new Helper();
            _security = new Security();
            _contract_version = version;
            _contract_hash = GetContractHash();
        }

        #endregion Constructor/Destructor

        /// <summary>
        /// Get available currency pairs on Switcheo Exchange 
        /// </summary>
        /// <param name="bases">Base pairs to filter on (default all pairs)</param>
        /// <returns>Array of trading pairs</returns>
        public async Task<string[]> GetPairs(string[] bases = null)
        {
            var endpoint = "/v2/pairs";

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
        /// Get hashes of contracts deployed by Switcheo
        /// </summary>
        /// <returns>Contracts dictionary</returns>
        public async Task<Dictionary<string, Dictionary<string, string>>> GetContracts()
        {
            var endpoint = "/v2/contracts";

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
                endTime = _helper.UTCtoUnixTime();
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
                var from = _helper.UTCtoUnixTime((DateTimeOffset)fromDate);
                querystring.Add($"from={from}");
            }
            if (toDate != null)
            {
                var to = _helper.UTCtoUnixTime((DateTimeOffset)toDate);
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
        /// Get contract balance of a given address
        /// </summary>
        /// <param name="address">String of addresses</param>
        /// <returns>Balance response</returns>
        public async Task<BalanceResponse> GetBalances(string address)
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
        /// Create a deposit
        /// </summary>
        /// <param name="asset">Asset to deposit</param>
        /// <param name="amount">Amount to deposit</param>
        /// <returns>Deposit response</returns>
        public async Task<TransactionResponse> CreateDeposit(string asset, decimal amount)
        {
            var endpoint = "/v2/deposits";

            var param = new DepositWithdrawalParams
            {
                amount = amount,
                asset_id = asset,
                blockchain = "neo",
                contract_hash = _contract_hash,
                timestamp = _helper.UTCtoUnixTimeMilliseconds()
            };

            var data = new DepositWithdrawalData(param);
            data.address = _address;
            data.signature = _security.CreateSignature(param, _key);

            var url = _baseUrl + endpoint;

            try
            {
                var response = await _restRepo.PostApi<TransactionResponse, DepositWithdrawalData>(url, data);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Execute a deposit
        /// </summary>
        /// <param name="deposit">Deposit detail from creation</param>
        /// <returns>Deposit response</returns>
        public async Task<TransactionResponse> ExecuteDeposit(TransactionResponse deposit)
        {
            var sigDic = new Dictionary<string, string>();
            sigDic.Add("signature", _security.CreateSignature(deposit.transaction, _key));
            var endpoint = "/v2/deposits";
            
            var url = _baseUrl + endpoint + $"/{deposit.id}/broadcast";

            try
            {
                var response = await _restRepo.PostApi<TransactionResponse, Dictionary<string, string>>(url, sigDic);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Create a withdrawal
        /// </summary>
        /// <param name="asset">Asset to withdrawal</param>
        /// <param name="amount">Amount to withdrawal</param>
        /// <returns>Dictionary of string keys and values</returns>
        public async Task<Dictionary<string, string>> CreateWithdrawal(string asset, decimal amount)
        {
            var endpoint = "/v2/withdrawals";

            var param = new DepositWithdrawalParams
            {
                amount = amount,
                asset_id = asset,
                blockchain = "neo",
                contract_hash = _contract_hash,
                timestamp = _helper.UTCtoUnixTimeMilliseconds()
            };

            var data = new DepositWithdrawalData(param);
            data.address = _address;
            data.signature = _security.CreateSignature(param, _key);

            var url = _baseUrl + endpoint;

            try
            {
                var response = await _restRepo.PostApi<Dictionary<string, string>, DepositWithdrawalData>(url, data);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Execute a withdrawal
        /// </summary>
        /// <param name="withdrawalId">Guid of withdrawal request</param>
        /// <param name="signature">Signature from withdrawal creation</param>
        /// <returns>Withdrawal response</returns>
        public async Task<WithdrawalResponse> ExecuteWithdrawal(Guid withdrawalId, string signature)
        {
            var sigDic = new Dictionary<string, string>();
            sigDic.Add("id", withdrawalId.ToString());
            sigDic.Add("timestamp", _helper.UTCtoUnixTimeMilliseconds().ToString());

            sigDic.Add("signature", _security.CreateSignature(sigDic, _key));

            var endpoint = "/v2/withdrawals";

            var url = _baseUrl + endpoint + $"/{withdrawalId}/broadcast";

            try
            {
                var response = await _restRepo.PostApi<WithdrawalResponse, Dictionary<string, string>>(url, sigDic);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
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

        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <param name="pair">String of pair to match</param>
        /// <returns>Array of orders</returns>
        private async Task<Order[]> OnGetOrders(string address, string pair = "")
        {
            var endpoint = "/v2/orders";

            var queryString = $"?address={address}";
            if (pair != "")
            {
                queryString += $"&pair={pair}";
            }

            var url = _baseUrl + endpoint + queryString;

            try
            {
                var response = await _restRepo.GetApiStream<Order[]>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
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
        public async Task<Order> CreateOrder(string pair, Side side, decimal price, decimal amount, bool useSWTH = true)
        {
            var endpoint = "/v2/orders";
            var url = _baseUrl + endpoint;

            var request = new OrderRequest
            {
                blockchain = "neo",
                contract_hash = _contract_hash,
                order_type = "limit",
                pair = pair,
                price = price,
                side = side.ToString(),
                timestamp = _helper.UTCtoUnixTimeMilliseconds(),
                use_native_tokens = useSWTH,
                want_amount = amount
            };

            var signature = _security.CreateSignature(request, _key);

            var signedRequest = new OrderRequestSigned(request)
            {
                address = _address,
                signature = signature
            };
            
            try
            {
                var response = await _restRepo.PostApi<Order, OrderRequestSigned>(url, signedRequest);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This is the second endpoint required to execute an order. 
        /// After using the CreateOrder endpoint, 
        /// you will receive a response which needs to be signed.
        /// </summary>
        /// <param name="order">Order created</param>
        /// <returns>Boolean when complete</returns>
        public async Task<bool?> BroadcastOrder(Order order)
        {
            var endpoint = $"/v2/orders/{order.id}/broadcast";
            var url = _baseUrl + endpoint;

            var signatures = new OrderSignatures
            {
                fills = SignFills(order.fills),
                makes = SignMakes(order.makes)
            };

            try
            {
                var response = await _restRepo.PostApi<bool, OrderSignatures>(url, signatures);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
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
            var endpoint = $"/v2/cancellations";
            var url = _baseUrl + endpoint;

            var sigDic = new Dictionary<string, string>();
            sigDic.Add("order_id", order.id.ToString());
            sigDic.Add("timestamp", _helper.UTCtoUnixTimeMilliseconds().ToString());

            sigDic.Add("signature", _security.CreateSignature(sigDic, _key));
            sigDic.Add("address", _address);

            try
            {
                var response = await _restRepo.PostApi<TransactionResponse, Dictionary<string, string>>(url, sigDic);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This is the second endpoint that must be called to cancel an order. 
        /// After calling the CreateCancellation endpoint, 
        /// you will receive a transaction in the response which must be signed.
        /// </summary>
        /// <param name="order">Order to be cancelled</param>
        /// <returns>Boolean when complete</returns>
        public async Task<bool?> ExecuteCancellation(TransactionResponse cancellation)
        {
            var endpoint = $"/v2/cancellations/{cancellation.id}/broadcast";
            var url = _baseUrl + endpoint;

            var sigDic = new Dictionary<string, string>();
            sigDic.Add("signature", _security.CreateSignature(cancellation.transaction, _key));

            try
            {
                var response = await _restRepo.PostApi<bool, Dictionary<string, string>>(url, sigDic);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Sign fills in an order
        /// </summary>
        /// <param name="fills">Array of fills to sign</param>
        /// <returns>String array of signatures</returns>
        private string[] SignFills(Fill[] fills)
        {
            var sigList = new List<string>();

            for (int i = 0; i < fills.Length; i++)
            {
                sigList.Add(_security.CreateSignature(fills[i].txn, _key));
            }

            return sigList.ToArray();
        }

        /// <summary>
        /// Sign makes in an order
        /// </summary>
        /// <param name="makes">Array of makes to sign</param>
        /// <returns>String array of signatures</returns>
        private string[] SignMakes(Make[] makes)
        {
            var sigList = new List<string>();

            for (int i = 0; i < makes.Length; i++)
            {
                sigList.Add(_security.CreateSignature(makes[i].txn, _key));
            }

            return sigList.ToArray();
        }

        /// <summary>
        /// Gets latest or specified contract hash
        /// </summary>
        /// <returns>String of hash</returns>
        private string GetContractHash()
        {
            var contracts = GetContracts().Result;

            var versions = contracts["NEO"];

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
