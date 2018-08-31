using SwitcheoApi.NetCore.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitcheoApi.NetCore.Data.Interface
{
    public interface ISwitcheoRepository
    {
        /// <summary>
        /// Retrieve a list of supported tokens on Switcheo.
        /// </summary>
        /// <returns>Tokens dictionary</returns>
        Task<Dictionary<string, Token>> GetTokens();

        /// <summary>
        /// Get available currency pairs on Switcheo Exchange 
        /// </summary>
        /// <param name="bases">Base pairs to filter on (default all pairs)</param>
        /// <returns>Array of trading pairs</returns>
        Task<string[]> GetPairs(string[] bases = null);

        /// <summary>
        /// Get hashes of contracts deployed by Switcheo
        /// </summary>
        /// <returns>Contracts dictionary</returns>
        Task<Dictionary<string, Dictionary<string, string>>> GetContracts();

        /// <summary>
        /// Get candlestick chart data
        /// </summary>
        /// <param name="pair">Pair to filter</param>
        /// <param name="interval">Time interval of candlestick</param>
        /// <param name="endTime">End time of last candlestick (defaults to current UTC time)</param>
        /// <param name="stickCount">Number of candlesticks to return (defaults to 10)</param>
        /// <returns>Array of Candlesticks</returns>
        Task<Candlstick[]> GetCandlesticks(string pair, Interval interval, long endTime = 0, int stickCount = 10);

        /// <summary>
        /// Get candlestick chart data
        /// </summary>
        /// <param name="pair">Pair to filter</param>
        /// <param name="interval">Time interval of candlestick</param>
        /// <param name="endTime">Unix time of last candlestick</param>
        /// <param name="startTime">Unix time of 1st candlestick</param>
        /// <returns>Array of Candlesticks</returns>
        Task<Candlstick[]> GetCandleSticks(string pair, Interval interval, long endTime, long startTime);

        /// <summary>
        /// Get 24-hour data for all pairs and markets
        /// </summary>
        /// <returns>Array of Candlesticks</returns>
        Task<Candlstick[]> GetLast24Hours();

        /// <summary>
        /// Get last price of given symbol
        /// </summary>
        /// <param name="symbols">String array of currency symbols (default null)</param>
        /// <param name="bases">String array of base pairs (default null)</param>
        /// <returns>LastPrices dictionary</returns>
        Task<Dictionary<string, Dictionary<string, decimal>>> GetLastPrice(string[] symbols = null, string[] bases = null);

        /// <summary>
        /// Get best 70 offers on the offer book
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>Array of Offers</returns>
        Task<Offer[]> GetOffers(string pair);

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>Array of TradeDetail</returns>
        Task<TradeDetail[]> GetTrades(string pair);

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="tradeCount">Number of trades to return</param>
        /// <returns>Array of TradeDetail</returns>
        Task<TradeDetail[]> GetTrades(string pair, int tradeCount);

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="fromDate">Only return trades after this date (default = null)</param>
        /// <param name="toDate">Only return trades before this date (default = null)</param>
        /// <returns>Array of TradeDetail</returns>
        Task<TradeDetail[]> GetTrades(string pair, DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null);

        /// <summary>
        /// Get executed trades for a given pair
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <param name="fromDate">Only return trades after this date (default = null)</param>
        /// <param name="toDate">Only return trades before this date (default = null)</param>
        /// <param name="tradeCount">Number of trades to return (default = 10)</param>
        /// <returns>Array of TradeDetail</returns>
        Task<TradeDetail[]> GetTrades(string pair, DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null, int tradeCount = 5000);

        /// <summary>
        /// Get loaded wallet
        /// </summary>
        /// <returns>NeoWallet if loaded</returns>
        NeoWallet GetWallet();

        /// <summary>
        /// Get contract balance for signed in user
        /// </summary>
        /// <returns>Balance response</returns>
        Task<BalanceResponse> GetBalances();

        /// <summary>
        /// Get contract balance of a given address
        /// </summary>
        /// <param name="address">String of addresses</param>
        /// <returns>Balance response</returns>
        Task<BalanceResponse> GetBalances(string address);

        /// <summary>
        /// Create and execute a deposit to the exchange
        /// </summary>
        /// <param name="asset">Asset to deposit</param>
        /// <param name="amount">Amount to deposit</param>
        /// <returns>Boolean when complete</returns>
        Task<bool> Deposit(string asset, decimal amount);

        /// <summary>
        /// Post a deposit
        /// </summary>
        /// <param name="asset">Asset to deposit</param>
        /// <param name="amount">Amount to deposit</param>
        /// <returns>Deposit response</returns>
        Task<TransactionResponse> CreateDeposit(string asset, decimal amount);

        /// <summary>
        /// Execute a deposit
        /// </summary>
        /// <param name="deposit">Deposit detail from creation</param>
        /// <returns>Deposit response</returns>
        Task<TransactionResponse> ExecuteDeposit(TransactionResponse deposit);

        /// <summary>
        /// Create and execute a withdrawal from the exchange
        /// </summary>
        /// <param name="asset">Asset to withdrawal</param>
        /// <param name="amount">Amount to withdrawal</param>
        /// <returns>Boolean when complete</returns>
        Task<bool> Withdrawal(string asset, decimal amount);

        /// <summary>
        /// Create a withdrawal
        /// </summary>
        /// <param name="asset">Asset to withdrawal</param>
        /// <param name="amount">Amount to withdrawal</param>
        /// <returns>Withdrawal id</returns>
        Task<string> CreateWithdrawal(string asset, decimal amount);

        /// <summary>
        /// Execute a withdrawal
        /// </summary>
        /// <param name="withdrawalId">Guid of withdrawal request</param>
        /// <returns>Withdrawal response</returns>
        Task<WithdrawalResponse> ExecuteWithdrawal(string withdrawalId);
        
        /// <summary>
        /// Get orders for current address
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <returns>Array of orders</returns>
        Task<Order[]> GetOrders();

        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <returns>Array of orders</returns>
        Task<Order[]> GetOrders(string address);

        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="address">Address with orders</param>
        /// <param name="pair">String of pair to match</param>
        /// <returns>Array of orders</returns>
        Task<Order[]> GetOrders(string address, string pair);

        /// <summary>
        /// This endpoint creates an order which can be executed through BroadcastOrder.
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="neoAmount">Decimal amount of NEO to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        Task<Order> CreateNeoOrder(string pair, Side side, decimal price, decimal neoAmount, bool useSWTH = true);

        /// <summary>
        /// This endpoint creates an order which can be executed through BroadcastOrder.
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="tokenAmount">Decimal amount of tokens to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        Task<Order> CreateTokenOrder(string pair, Side side, decimal price, decimal tokenAmount, bool useSWTH = true);

        /// <summary>
        /// This endpoint places an order using Neo amount
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="neoAmount">Decimal amount of NEO to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        Task<Order> PlaceNeoOrder(string pair, Side side, decimal price, decimal neoAmount, bool useSWTH = true);

        /// <summary>
        /// This endpoint places an order using token amount
        /// </summary>
        /// <param name="pair">String of pair to match</param>
        /// <param name="side">Buy or Sell</param>
        /// <param name="price">Decimal of order price</param>
        /// <param name="amount">Decimal amount of tokens to buy/sell</param>
        /// <param name="useSWTH">Boolean to use SWTH for fees</param>
        /// <returns>Order object</returns>
        Task<Order> PlaceOrder(string pair, Side side, decimal price, decimal amount, bool useSWTH = true);

        /// <summary>
        /// This is the second endpoint required to execute an order. 
        /// After using the CreateOrder endpoint, 
        /// you will receive a response which needs to be signed.
        /// </summary>
        /// <param name="order">Order created</param>
        /// <returns>Boolean when complete</returns>
        Task<Order> BroadcastOrder(Order order);

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="orderId">Id of order to cancel</param>
        /// <returns>Cancelled Order object</returns>
        Task<Order> CancelOrder(string orderId);

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="order">Order to cancel</param>
        /// <returns>Cancelled Order object</returns>
        Task<Order> CancelOrder(Order order);

        /// <summary>
        /// This is the first API call required to cancel an order. 
        /// Only orders with makes and with 
        /// an available_amount of more than 0 can be cancelled.
        /// </summary>
        /// <param name="orderId">Order Id to be cancelled</param>
        /// <returns>TransactionResponse when complete</returns>
        Task<TransactionResponse> CreateCancellation(string orderId);

        /// <summary>
        /// This is the first API call required to cancel an order. 
        /// Only orders with makes and with 
        /// an available_amount of more than 0 can be cancelled.
        /// </summary>
        /// <param name="order">Order to be cancelled</param>
        /// <returns>TransactionResponse when complete</returns>
        Task<TransactionResponse> CreateCancellation(Order order);

        /// <summary>
        /// This is the second endpoint that must be called to cancel an order. 
        /// After calling the CreateCancellation endpoint, 
        /// you will receive a transaction in the response which must be signed.
        /// </summary>
        /// <param name="cancellation">Cancellation object</param>
        /// <returns>Order object</returns>
        Task<Order> ExecuteCancellation(TransactionResponse cancellation);
        
        /// <summary>
        /// Get server timestamp
        /// </summary>
        /// <returns>Long of timestamp</returns>
        Task<long> GetServerTime();
    }
}
