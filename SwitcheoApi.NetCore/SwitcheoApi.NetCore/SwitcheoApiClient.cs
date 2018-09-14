using SwitcheoApi.NetCore.Data;
using SwitcheoApi.NetCore.Data.Interface;
using SwitcheoApi.NetCore.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Switcheo.NetCore
{
    public class SwitcheoApiClient
    {
        [Obsolete("This initializer is depricated. Use direct method calls instead.")]
        public ISwitcheoRepository _switcheoRepository;
        public ISwitcheoRepository _repository;

        /// <summary>
        /// Constructor, no authorization
        /// </summary>
        public SwitcheoApiClient()
        {
            _switcheoRepository = new SwitcheoRepository(false);
            _repository = new SwitcheoRepository(false);
        }

        /// <summary>
        /// Constructor, with authorization
        /// </summary>
        /// <param name="loginValue">Neo public address or private key</param>
        public SwitcheoApiClient(string loginValue)
        {
            _repository = new SwitcheoRepository(loginValue, false, "", Blockchain.neo);
        }

        /// <summary>
        /// Constructor, with authorization
        /// </summary>
        /// <param name="loginValue">Neo public address or private key</param>
        /// <param name="blockchain">Blockchain (neo, eth, qtum)</param>
        public SwitcheoApiClient(string loginValue, Blockchain blockchain)
        {
            _repository = new SwitcheoRepository(loginValue, false, "", blockchain);
        }

        /// <summary>
        /// Constructor, no authorization
        /// </summary>
        /// <param name="testRegion">Boolean to use test region (default = false)</param>
        /// <param name="version">Contract version (default latest)</param>
        /// <param name="blockchain">Blockchain (default = neo)</param>
        public SwitcheoApiClient(bool testRegion = false, string version = "", Blockchain blockchain = Blockchain.neo)
        {
            _switcheoRepository = new SwitcheoRepository(testRegion, version, blockchain);
            _repository = new SwitcheoRepository(testRegion, version, blockchain);
        }

        /// <summary>
        /// Constructor, with authorization
        /// </summary>
        /// <param name="loginValue">Neo public address or private key</param>
        /// <param name="testRegion">Boolean to use test region (default = false)</param>
        /// <param name="version">Contract version (default latest)</param>
        /// <param name="blockchain">Blockchain (default = neo)</param>
        public SwitcheoApiClient(string loginValue, bool testRegion = false, Blockchain blockchain = Blockchain.neo)
        {
            _repository = new SwitcheoRepository(loginValue, testRegion, "", blockchain);
        }

        /// <summary>
        /// Constructor, with authorization
        /// </summary>
        /// <param name="loginValue">Neo public address or private key</param>
        /// <param name="testRegion">Boolean to use test region (default = false)</param>
        /// <param name="version">Contract version (default latest)</param>
        /// <param name="blockchain">Blockchain (default = neo)</param>
        public SwitcheoApiClient(string loginValue, bool testRegion = false, string version = "", Blockchain blockchain = Blockchain.neo)
        {
            _repository = new SwitcheoRepository(loginValue, testRegion, version, blockchain);
        }

        /// <summary>
        /// Get a wallet, if loaded
        /// </summary>
        /// <returns>NeoWallet object</returns>
        public NeoWallet GetWallet()
        {
            return _repository.GetWallet();
        }

        /// <summary>
        /// Retrieve a list of supported tokens on Switcheo.
        /// </summary>
        /// <returns>Tokens dictionary</returns>
        public Dictionary<string, Token> GetTokens()
        {
            return _repository.GetTokens();
        }

        /// <summary>
        /// Get available currency pairs on Switcheo Exchange 
        /// </summary>
        /// <param name="bases">Base pairs to filter on (default all pairs)</param>
        /// <returns>Array of trading pairs</returns>
        public string[] GetPairs(string[] bases = null)
        {
            return _repository.GetPairs(bases).Result;
        }

        /// <summary>
        /// Get hashes of contracts deployed by Switcheo
        /// </summary>
        /// <returns>Contracts dictionary</returns>
        public Dictionary<string, Dictionary<string, string>> GetContracts()
        {
            return _repository.GetContracts().Result;
        }

        /// <summary>
        /// Get candlestick chart data
        /// </summary>
        /// <param name="pair">Pair to filter</param>
        /// <param name="interval">Time interval of candlestick</param>
        /// <param name="endTime">End time of last candlestick (defaults to current UTC time)</param>
        /// <param name="stickCount">Number of candlesticks to return (defaults to 10)</param>
        /// <returns>Array of Candlesticks</returns>
        public Candlstick[] GetCandlesticks(string pair, Interval interval, long endTime = 0, int stickCount = 10)
        {
            return _repository.GetCandlesticks(pair, interval, endTime, stickCount).Result;
        }

        /// <summary>
        /// Get candlestick chart data
        /// </summary>
        /// <param name="pair">Pair to filter</param>
        /// <param name="interval">Time interval of candlestick</param>
        /// <param name="endTime">Unix time of last candlestick</param>
        /// <param name="startTime">Unix time of 1st candlestick</param>
        /// <returns>Array of Candlesticks</returns>
        public Candlstick[] GetCandleSticks(string pair, Interval interval, long endTime, long startTime)
        {
            return _repository.GetCandleSticks(pair, interval, endTime, startTime).Result;
        }

        /// <summary>
        /// Get 24-hour data for all pairs and markets
        /// </summary>
        /// <returns>Array of Candlesticks</returns>
        public Candlstick[] GetLast24Hours()
        {
            return _repository.GetLast24Hours().Result;
        }

        /// <summary>
        /// Get last price of given symbol
        /// </summary>
        /// <param name="symbols">String array of currency symbols (default null)</param>
        /// <param name="bases">String array of base pairs (default null)</param>
        /// <returns>LastPrices dictionary</returns>
        public Dictionary<string, Dictionary<string, decimal>> GetLastPrice(string[] symbols = null, string[] bases = null)
        {
            return _repository.GetLastPrice(symbols, bases).Result;
        }

        /// <summary>
        /// Get last price for a trading pair
        /// </summary>
        /// <param name="pair">String of trading pair</param>
        /// <returns>Decimal of last price</returns>
        public decimal GetLastPrice(string pair)
        {
            return _repository.GetLastPrice(pair).Result;
        }

        /// <summary>
        /// Get best 70 offers on the offer book with converted values
        /// Currently only returning asks
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>OrderBook object</returns>
        public OrderBook GetOrderBook(string pair)
        {
            return _repository.GetOrderBook(pair).Result;
        }

        /// <summary>
        /// Get best 70 offers on the offer book
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>Array of Offers</returns>
        public Offer[] GetOffers(string pair)
        {
            return _repository.GetOffers(pair).Result;
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>Array of TradeDetail</returns>
        public TradeDetail[] GetTrades(string pair)
        {
            return _repository.GetTrades(pair).Result;
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="tradeCount">Number of trades to return</param>
        /// <returns>Array of TradeDetail</returns>
        public TradeDetail[] GetTrades(string pair, int tradeCount)
        {
            return _repository.GetTrades(pair, tradeCount).Result;
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="fromDate">Only return trades after this date (default = null)</param>
        /// <param name="toDate">Only return trades before this date (default = null)</param>
        /// <returns>Array of TradeDetail</returns>
        public TradeDetail[] GetTrades(string pair, DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null)
        {
            return _repository.GetTrades(pair, fromDate, toDate).Result;
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="fromDate">Only return trades after this date (default = null)</param>
        /// <param name="toDate">Only return trades before this date (default = null)</param>
        /// <param name="tradeCount">Number of trades to return (default = 10)</param>
        /// <returns>Array of TradeDetail</returns>
        public TradeDetail[] GetTrades(string pair, DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null, int tradeCount = 5000)
        {
            return _repository.GetTrades(pair, fromDate, toDate, tradeCount).Result;
        }

        /// <summary>
        /// Get contract balance for signed in user
        /// </summary>
        /// <returns>Balance response</returns>
        public BalanceResponse GetBalances()
        {
            return _repository.GetBalances().Result;
        }

        /// <summary>
        /// Get contract balance of a given script hash (address)
        /// </summary>
        /// <param name="scriptHash">String of script hash</param>
        /// <returns>Balance response</returns>
        public BalanceResponse GetBalances(string scriptHash)
        {
            return _repository.GetBalances(scriptHash).Result;
        }

        /// <summary>
        /// Create and execute a deposit to the exchange
        /// </summary>
        /// <param name="asset">Asset to deposit</param>
        /// <param name="amount">Amount to deposit</param>
        /// <returns>Boolean when complete</returns>
        public bool Deposit(string asset, decimal amount)
        {
            return _repository.Deposit(asset, amount).Result;
        }

        /// <summary>
        /// Create a deposit
        /// It can be executed through ExecuteDeposit.
        /// EXECUTE REQUIRED
        /// </summary>
        /// <param name="asset">Asset to deposit</param>
        /// <param name="amount">Amount to deposit</param>
        /// <returns>Deposit response</returns>
        public TransactionResponse CreateDeposit(string asset, decimal amount)
        {
            return _repository.CreateDeposit(asset, amount).Result;
        }

        /// <summary>
        /// Execute a deposit
        /// </summary>
        /// <param name="deposit">Deposit detail from creation</param>
        /// <returns>Deposit response</returns>
        public TransactionResponse ExecuteDeposit(TransactionResponse deposit)
        {
            return _repository.ExecuteDeposit(deposit).Result;
        }

        /// <summary>
        /// Create and execute a withdrawal from the exchange
        /// </summary>
        /// <param name="asset">Asset to withdrawal</param>
        /// <param name="amount">Amount to withdrawal</param>
        /// <returns>Boolean when complete</returns>
        public bool Withdrawal(string asset, decimal amount)
        {
            return _repository.Withdrawal(asset, amount).Result;
        }

        /// <summary>
        /// Create a withdrawal
        /// It can be executed through ExecuteWithdrawal.
        /// EXECUTE REQUIRED
        /// </summary>
        /// <param name="asset">Asset to withdrawal</param>
        /// <param name="amount">Amount to withdrawal</param>
        /// <returns>Withdrawal id</returns>
        public string CreateWithdrawal(string asset, decimal amount)
        {
            return _repository.CreateWithdrawal(asset, amount).Result;
        }

        /// <summary>
        /// Execute a withdrawal
        /// </summary>
        /// <param name="withdrawalId">Guid of withdrawal request</param>
        /// <returns>Withdrawal response</returns>
        public WithdrawalResponse ExecuteWithdrawal(string withdrawalId)
        {
            return _repository.ExecuteWithdrawal(withdrawalId).Result;
        }

        /// <summary>
        /// Get an Order by Id
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Order object</returns>
        public Order GetOrder(string id)
        {
            return _repository.GetOrder(id).Result;
        }

        /// <summary>
        /// Get all open Orders
        /// </summary>
        /// <returns>Array of Order objects</returns>
        public Order[] GetOpenOrders()
        {
            return _repository.GetOpenOrders().Result;
        }

        /// <summary>
        /// Get all completed Orders
        /// </summary>
        /// <returns>Array of Order objects</returns>
        public Order[] GetCompletedOrders()
        {
            return _repository.GetCompletedOrders().Result;
        }

        /// <summary>
        /// Get orders for current address
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <returns>Array of orders</returns>
        public Order[] GetOrders()
        {
            return _repository.GetOrders().Result;
        }

        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <returns>Array of orders</returns>
        public Order[] GetOrders(string address)
        {
            return _repository.GetOrders(address).Result;
        }
        
        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <param name="pair">String of pair to match</param>
        /// <returns>Array of orders</returns>
        public Order[] GetOrders(string address, string pair)
        {
            return _repository.GetOrders(address, pair).Result;
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
        public Order PlaceNeoOrder(string pair, Side side, decimal price, decimal neoAmount, bool useSWTH = true)
        {
            return _repository.PlaceNeoOrder(pair, side, price, neoAmount, useSWTH).Result;
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
        public Order PlaceOrder(string pair, Side side, decimal price, decimal amount, bool useSWTH = true)
        {
            return _repository.PlaceNeoOrder(pair, side, price, amount, useSWTH).Result;
        }

        /// <summary>
        /// This endpoint creates a Neo order which can be executed through BroadcastOrder.
        /// BROADCAST REQUIRED
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="neoAmount">Decimal amount of NEO to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        public Order CreateNeoOrder(string pair, Side side, decimal price, decimal neoAmount, bool useSWTH = true)
        {
            return _repository.CreateNeoOrder(pair, side, price, neoAmount, useSWTH).Result;
        }

        /// <summary>
        /// This endpoint creates a Token order which can be executed through BroadcastOrder.
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="tokenAmount">Decimal amount of tokens to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        public Order CreateTokenOrder(string pair, Side side, decimal price, decimal tokenAmount, bool useSWTH = true)
        {
            return _repository.CreateTokenOrder(pair, side, price, tokenAmount, useSWTH).Result;
        }

        /// <summary>
        /// This is the second endpoint required to execute an order. 
        /// After using the CreateOrder endpoint, 
        /// you will receive a response which needs to be signed.
        /// </summary>
        /// <param name="order">Order created</param>
        /// <returns>Boolean when complete</returns>
        public Order BroadcastOrder(Order order)
        {
            return _repository.BroadcastOrder(order).Result;
        }

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="orderId">Id of order to cancel</param>
        /// <returns>Cancelled Order object</returns>
        public Order CancelOrder(string orderId)
        {
            return _repository.CancelOrder(orderId).Result;
        }

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="order">Order to cancel</param>
        /// <returns>Cancelled Order object</returns>
        public Order CancelOrder(Order order)
        {
            return _repository.CancelOrder(order).Result;
        }

        /// <summary>
        /// This is the first API call required to cancel an order. 
        /// Only orders with makes and with 
        /// an available_amount of more than 0 can be cancelled.
        /// It can be executed through ExecuteCancellation.
        /// EXECUTE REQUIRED
        /// </summary>
        /// <param name="orderId">Order Id to be cancelled</param>
        /// <returns>TransactionResponse when complete</returns>
        public TransactionResponse CreateCancellation(string orderId)
        {
            return _repository.CreateCancellation(orderId).Result;
        }

        /// <summary>
        /// This is the first API call required to cancel an order. 
        /// Only orders with makes and with 
        /// an available_amount of more than 0 can be cancelled. 
        /// It can be executed through ExecuteCancellation.
        /// EXECUTE REQUIRED
        /// </summary>
        /// <param name="order">Order to be cancelled</param>
        /// <returns>TransactionResponse when complete</returns>
        public TransactionResponse CreateCancellation(Order order)
        {
            return _repository.CreateCancellation(order).Result;
        }

        /// <summary>
        /// This is the second endpoint that must be called to cancel an order. 
        /// After calling the CreateCancellation endpoint, 
        /// you will receive a transaction in the response which must be signed.
        /// </summary>
        /// <param name="cancellation">Cancellation object</param>
        /// <returns>Order object</returns>
        public Order ExecuteCancellation(TransactionResponse cancellation)
        {
            return _repository.ExecuteCancellation(cancellation).Result;
        }

        /// <summary>
        /// Get server timestamp
        /// </summary>
        /// <returns>Long of timestamp</returns>
        public long GetServerTime()
        {
            return _repository.GetServerTime().Result;
        }

        /// <summary>
        /// Get available currency pairs on Switcheo Exchange 
        /// </summary>
        /// <param name="bases">Base pairs to filter on (default all pairs)</param>
        /// <returns>Array of trading pairs</returns>
        public async Task<string[]> GetPairsAsync(string[] bases = null)
        {
            return await _repository.GetPairs(bases);
        }

        /// <summary>
        /// Get hashes of contracts deployed by Switcheo
        /// </summary>
        /// <returns>Contracts dictionary</returns>
        public async Task<Dictionary<string, Dictionary<string, string>>> GetContractsAsync()
        {
            return await _repository.GetContracts();
        }

        /// <summary>
        /// Get candlestick chart data
        /// </summary>
        /// <param name="pair">Pair to filter</param>
        /// <param name="interval">Time interval of candlestick</param>
        /// <param name="endTime">End time of last candlestick (defaults to current UTC time)</param>
        /// <param name="stickCount">Number of candlesticks to return (defaults to 10)</param>
        /// <returns>Array of Candlesticks</returns>
        public async Task<Candlstick[]> GetCandlesticksAsync(string pair, Interval interval, long endTime = 0, int stickCount = 10)
        {
            return await _repository.GetCandlesticks(pair, interval, endTime, stickCount);
        }

        /// <summary>
        /// Get candlestick chart data
        /// </summary>
        /// <param name="pair">Pair to filter</param>
        /// <param name="interval">Time interval of candlestick</param>
        /// <param name="endTime">Unix time of last candlestick</param>
        /// <param name="startTime">Unix time of 1st candlestick</param>
        /// <returns>Array of Candlesticks</returns>
        public async Task<Candlstick[]> GetCandleSticksAsync(string pair, Interval interval, long endTime, long startTime)
        {
            return await _repository.GetCandleSticks(pair, interval, endTime, startTime);
        }

        /// <summary>
        /// Get 24-hour data for all pairs and markets
        /// </summary>
        /// <returns>Array of Candlesticks</returns>
        public async Task<Candlstick[]> GetLast24HoursAsync()
        {
            return await _repository.GetLast24Hours();
        }

        /// <summary>
        /// Get last price of given symbol
        /// </summary>
        /// <param name="symbols">String array of currency symbols (default null)</param>
        /// <param name="bases">String array of base pairs (default null)</param>
        /// <returns>LastPrices dictionary</returns>
        public async Task<Dictionary<string, Dictionary<string, decimal>>> GetLastPriceAsync(string[] symbols = null, string[] bases = null)
        {
            return await _repository.GetLastPrice(symbols, bases);
        }

        /// <summary>
        /// Get last price for a trading pair
        /// </summary>
        /// <param name="pair">String of trading pair</param>
        /// <returns>Decimal of last price</returns>
        public async Task<decimal> GetLastPriceAsync(string pair)
        {
            return await _repository.GetLastPrice(pair);
        }

        /// <summary>
        /// Get best 70 offers on the offer book with converted values
        /// Currently only returning asks
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>OrderBook object</returns>
        public async Task<OrderBook> GetOrderBookAsync(string pair)
        {
            return await _repository.GetOrderBook(pair);
        }

        /// <summary>
        /// Get best 70 offers on the offer book
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>Array of Offers</returns>
        public async Task<Offer[]> GetOffersAsync(string pair)
        {
            return await _repository.GetOffers(pair);
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>Array of TradeDetail</returns>
        public async Task<TradeDetail[]> GetTradesAsync(string pair)
        {
            return await _repository.GetTrades(pair);
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="tradeCount">Number of trades to return</param>
        /// <returns>Array of TradeDetail</returns>
        public async Task<TradeDetail[]> GetTradesAsync(string pair, int tradeCount)
        {
            return await _repository.GetTrades(pair, tradeCount);
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="fromDate">Only return trades after this date (default = null)</param>
        /// <param name="toDate">Only return trades before this date (default = null)</param>
        /// <returns>Array of TradeDetail</returns>
        public async Task<TradeDetail[]> GetTradesAsync(string pair, DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null)
        {
            return await _repository.GetTrades(pair, fromDate, toDate);
        }

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="fromDate">Only return trades after this date (default = null)</param>
        /// <param name="toDate">Only return trades before this date (default = null)</param>
        /// <param name="tradeCount">Number of trades to return (default = 10)</param>
        /// <returns>Array of TradeDetail</returns>
        public async Task<TradeDetail[]> GetTradesAsync(string pair, DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null, int tradeCount = 5000)
        {
            return await _repository.GetTrades(pair, fromDate, toDate, tradeCount);
        }

        /// <summary>
        /// Get contract balance for signed in user
        /// </summary>
        /// <returns>Balance response</returns>
        public async Task<BalanceResponse> GetBalancesAsync()
        {
            return await _repository.GetBalances();
        }

        /// <summary>
        /// Get contract balance of a given script hash
        /// </summary>
        /// <param name="scriptHash">String of script hash</param>
        /// <returns>Balance response</returns>
        public async Task<BalanceResponse> GetBalancesAsync(string scriptHash)
        {
            return await _repository.GetBalances(scriptHash);
        }

        /// <summary>
        /// Create and execute a deposit to the exchange
        /// </summary>
        /// <param name="asset">Asset to deposit</param>
        /// <param name="amount">Amount to deposit</param>
        /// <returns>Boolean when complete</returns>
        public async Task<bool> DepositAsync(string asset, decimal amount)
        {
            return await _repository.Deposit(asset, amount);
        }

        /// <summary>
        /// Create a deposit
        /// It can be executed through ExecuteDeposit.
        /// EXECUTE REQUIRED
        /// </summary>
        /// <param name="asset">Asset to deposit</param>
        /// <param name="amount">Amount to deposit</param>
        /// <returns>Deposit response</returns>
        public async Task<TransactionResponse> CreateDepositAsync(string asset, decimal amount)
        {
            return await _repository.CreateDeposit(asset, amount);
        }

        /// <summary>
        /// Execute a deposit
        /// </summary>
        /// <param name="deposit">Deposit detail from creation</param>
        /// <returns>Deposit response</returns>
        public async Task<TransactionResponse> ExecuteDepositAsync(TransactionResponse deposit)
        {
            return await _repository.ExecuteDeposit(deposit);
        }

        /// <summary>
        /// Create and execute a withdrawal from the exchange
        /// </summary>
        /// <param name="asset">Asset to withdrawal</param>
        /// <param name="amount">Amount to withdrawal</param>
        /// <returns>Boolean when complete</returns>
        public async Task<bool> WithdrawalAsync(string asset, decimal amount)
        {
            return await _repository.Withdrawal(asset, amount);
        }

        /// <summary>
        /// Create a withdrawal
        /// It can be executed through ExecuteWithdrawal.
        /// EXECUTE REQUIRED
        /// </summary>
        /// <param name="asset">Asset to withdrawal</param>
        /// <param name="amount">Amount to withdrawal</param>
        /// <returns>Withdrawal id</returns>
        public async Task<string> CreateWithdrawalAsync(string asset, decimal amount)
        {
            return await _repository.CreateWithdrawal(asset, amount);
        }

        /// <summary>
        /// Execute a withdrawal
        /// </summary>
        /// <param name="withdrawalId">Guid of withdrawal request</param>
        /// <returns>Withdrawal response</returns>
        public async Task<WithdrawalResponse> ExecuteWithdrawalAsync(string withdrawalId)
        {
            return await _repository.ExecuteWithdrawal(withdrawalId);
        }

        /// <summary>
        /// Get an Order by Id
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Order object</returns>
        public async Task<Order> GetOrderAsync(string id)
        {
            return await _repository.GetOrder(id);
        }

        /// <summary>
        /// Get all open Orders
        /// </summary>
        /// <returns>Array of Order objects</returns>
        public async Task<Order[]> GetOpenOrdersAsync()
        {
            return await _repository.GetOpenOrders();
        }

        /// <summary>
        /// Get all completed Orders
        /// </summary>
        /// <returns>Array of Order objects</returns>
        public async Task<Order[]> GetCompletedOrdersAsync()
        {
            return await _repository.GetCompletedOrders();
        }

        /// <summary>
        /// Get orders for current address
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <returns>Array of orders</returns>
        public async Task<Order[]> GetOrdersAsync()
        {
            return await _repository.GetOrders();
        }

        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <returns>Array of orders</returns>
        public async Task<Order[]> GetOrdersAsync(string address)
        {
            return await _repository.GetOrders(address);
        }

        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <param name="pair">String of pair to match</param>
        /// <returns>Array of orders</returns>
        public async Task<Order[]> GetOrdersAsync(string address, string pair)
        {
            return await _repository.GetOrders(address, pair);
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
        public async Task<Order> PlaceNeoOrderAsync(string pair, Side side, decimal price, decimal neoAmount, bool useSWTH = true)
        {
            return await _repository.PlaceNeoOrder(pair, side, price, neoAmount, useSWTH);
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
        public async Task<Order> PlaceOrderAsync(string pair, Side side, decimal price, decimal amount, bool useSWTH = true)
        {
            return await _repository.PlaceOrder(pair, side, price, amount, useSWTH);
        }

        /// <summary>
        /// This endpoint creates a Neo order which can be executed through BroadcastOrder.
        /// BROADCAST REQUIRED
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="neoAmount">Decimal amount of NEO to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        public async Task<Order> CreateNeoOrderAsync(string pair, Side side, decimal price, decimal neoAmount, bool useSWTH = true)
        {
            return await _repository.CreateNeoOrder(pair, side, price, neoAmount, useSWTH);
        }

        /// <summary>
        /// This endpoint creates a Token order which can be executed through BroadcastOrder.
        /// BROADCAST REQUIRED
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="tokenAmount">Decimal amount of tokens to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        public async Task<Order> CreateTokenOrderAsync(string pair, Side side, decimal price, decimal tokenAmount, bool useSWTH = true)
        {
            return await _repository.CreateTokenOrder(pair, side, price, tokenAmount, useSWTH);
        }

        /// <summary>
        /// This is the second endpoint required to execute an order. 
        /// After using the CreateOrder endpoint, 
        /// you will receive a response which needs to be signed.
        /// </summary>
        /// <param name="order">Order created</param>
        /// <returns>Boolean when complete</returns>
        public async Task<Order> BroadcastOrderAsync(Order order)
        {
            return await _repository.BroadcastOrder(order);
        }

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="orderId">Id of order to cancel</param>
        /// <returns>Cancelled Order object</returns>
        public async Task<Order> CancelOrderAsync(string orderId)
        {
            return await _repository.CancelOrder(orderId);
        }

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="order">Order to cancel</param>
        /// <returns>Cancelled Order object</returns>
        public async Task<Order> CancelOrderAsync(Order order)
        {
            return await _repository.CancelOrder(order);
        }

        /// <summary>
        /// This is the first API call required to cancel an order. 
        /// Only orders with makes and with 
        /// an available_amount of more than 0 can be cancelled.
        /// It can be executed through ExecuteCancellation.
        /// EXECUTE REQUIRED
        /// </summary>
        /// <param name="orderId">Order Id to be cancelled</param>
        /// <returns>TransactionResponse when complete</returns>
        public async Task<TransactionResponse> CreateCancellationAsync(string orderId)
        {
            return await _repository.CreateCancellation(orderId);
        }

        /// <summary>
        /// This is the first API call required to cancel an order. 
        /// Only orders with makes and with 
        /// an available_amount of more than 0 can be cancelled.
        /// It can be executed through ExecuteCancellation.
        /// EXECUTE REQUIRED
        /// </summary>
        /// <param name="order">Order to be cancelled</param>
        /// <returns>TransactionResponse when complete</returns>
        public async Task<TransactionResponse> CreateCancellationAsync(Order order)
        {
            return await _repository.CreateCancellation(order);
        }

        /// <summary>
        /// This is the second endpoint that must be called to cancel an order. 
        /// After calling the CreateCancellation endpoint, 
        /// you will receive a transaction in the response which must be signed.
        /// </summary>
        /// <param name="cancellation">Cancellation object</param>
        /// <returns>Order object</returns>
        public async Task<Order> ExecuteCancellationAsync(TransactionResponse cancellation)
        {
            return await _repository.ExecuteCancellation(cancellation);
        }

        /// <summary>
        /// Get server timestamp
        /// </summary>
        /// <returns>Long of timestamp</returns>
        public async Task<long> GetServerTimeAsync()
        {
            return await _repository.GetServerTime();
        }
    }
}
