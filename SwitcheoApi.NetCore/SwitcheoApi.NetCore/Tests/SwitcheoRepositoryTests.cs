using DateTimeHelpers;
using SwitcheoApi.NetCore.Core;
using SwitcheoApi.NetCore.Data.Interface;
using SwitcheoApi.NetCore.Entities;
using SwitcheoApi.NetCore.Tests;
using System;
using System.Linq;
using Xunit;

namespace SwitcheoApi.NetCore.Data.Tests
{
    public class SwitcheoRepositoryTests
    {
        private ISwitcheoRepository _repo;
        private ISwitcheoRepository _repoAuth;
        private Helper _helper;
        private DateTimeHelper _dtHelper;
        // Empty neo wallet for testing purposes only
        private string _address;
        private string _privateKey;
        private string _scriptHash;

        public SwitcheoRepositoryTests()
        {
            TestObjects objs = new TestObjects();
            _address = objs.GetAddress();
            _privateKey = objs.GetWIF();
            _scriptHash = objs.GetScriptHash();
            _repo = new SwitcheoRepository();
            _repoAuth = new SwitcheoRepository(_privateKey, true);
            _helper = new Helper();
            _dtHelper = new DateTimeHelper();
        }

        [Fact]
        public void GetPairs_Test()
        {
            var pairs = _repo.GetPairs().Result;

            Assert.NotNull(pairs);
        }

        [Fact]
        public void GetSWTHPairs_Test()
        {
            var bases = new string[1] { "SWTH" };
            var pairs = _repo.GetPairs(bases).Result;

            Assert.NotNull(pairs);
        }

        [Fact]
        public void GetTokens_Test()
        {
            var tokens = _repo.GetTokens().Result;

            Assert.True(tokens.Count > 0);
        }

        [Fact]
        public void GetServerTime_Test()
        {
            var timestamp = _repo.GetServerTime().Result;

            Assert.True(timestamp > 0);
        }

        [Fact]
        public void GetContracts_Test()
        {
            var contracts = _repo.GetContracts().Result;

            Assert.NotNull(contracts);
        }

        [Fact]
        public void GetCandlesticksDefaults_Test()
        {
            var pair = "SWTH_NEO";
            var interval = Interval.OneD;
            var candlesticks = _repo.GetCandlesticks(pair, interval).Result;

            Assert.NotNull(candlesticks);
        }

        [Fact]
        public void GetCandlesticksWithValues_Test()
        {
            var pair = "SWTH_NEO";
            var interval = Interval.OneD;
            var endTime = _dtHelper.UTCtoUnixTime();
            var sticksToReturn = 2;
            var candlesticks = _repo.GetCandlesticks(pair, interval, endTime, sticksToReturn).Result;

            Assert.True(candlesticks.Length == sticksToReturn);
        }

        [Fact]
        public void GetCandlesticksWithDateValues_Test()
        {
            var pair = "SWTH_NEO";
            var interval = Interval.OneD;
            var now = DateTime.UtcNow;
            var endTime = _dtHelper.UTCtoUnixTime(now);
            var tenDays = now.AddDays(-10);
            var startTime = _dtHelper.UTCtoUnixTime(tenDays);
            var candlesticks = _repo.GetCandleSticks(pair, interval, endTime, startTime).Result;

            Assert.NotNull(candlesticks);
        }

        [Fact]
        public void GetLast24Hours_Test()
        {
            var candlesticks = _repo.GetLast24Hours().Result;

            Assert.NotNull(candlesticks);
        }

        [Fact]
        public void GetLastPriceNoValues_Test()
        {
            var prices = _repo.GetLastPrice().Result;

            Assert.True(prices.Count > 0);
        }

        [Fact]
        public void GetLastPriceWithSymbols_Test()
        {
            var symbols = new string[1] { "SWTH" };
            var prices = _repo.GetLastPrice(symbols).Result;

            Assert.True(prices.Count > 0);
        }

        [Fact]
        public void GetLastPriceWithBases_Test()
        {
            var bases = new string[1] { "SWTH" };
            var prices = _repo.GetLastPrice(null, bases).Result;

            Assert.True(prices.Count > 0);
        }

        [Fact]
        public void GetLastPrice_Test()
        {
            var pair = "SWTH_NEO";
            var price = _repo.GetLastPrice(pair).Result;

            Assert.True(price > 0);
        }

        [Fact]
        public void GetOrderBook_Test()
        {
            var pair = "SWTH_NEO";
            var offers = _repo.GetOrderBook(pair).Result;

            Assert.NotNull(offers);
        }

        [Fact]
        public void GetOffers_Test()
        {
            var pair = "SWTH_NEO";
            var offers = _repo.GetOffers(pair).Result;

            Assert.NotNull(offers);
        }

        [Fact]
        public void GetTradesDefaults_Test()
        {
            var pair = "SWTH_NEO";
            var trades = _repo.GetTrades(pair).Result;

            Assert.NotNull(trades);
        }

        [Fact]
        public void GetTradesBetweenDates_Test()
        {
            var pair = "SWTH_NEO";
            var from = new DateTimeOffset(2018, 7, 17, 2, 18, 00, TimeSpan.FromSeconds(0));
            var to = new DateTimeOffset(2018, 7, 17, 2, 22, 00, TimeSpan.FromSeconds(0));
            var trades = _repo.GetTrades(pair, from, to).Result;

            Assert.NotNull(trades);
        }

        [Fact]
        public void GetTradesBadDateRange_Test()
        {
            var pair = "SWTH_NEO";
            var from = new DateTimeOffset(2018, 7, 17, 2, 22, 00, TimeSpan.FromSeconds(0));
            var to = new DateTimeOffset(2018, 7, 17, 2, 18, 00, TimeSpan.FromSeconds(0));
            Action act = () => _repo.GetTrades(pair, from, to);

            Assert.Throws<Exception>(act);
        }

        [Fact]
        public void GetTradesFromDateOnly_Test()
        {
            var pair = "SWTH_NEO";
            var from = new DateTimeOffset(2018, 7, 17, 2, 18, 00, TimeSpan.FromSeconds(0));
            var trades = _repo.GetTrades(pair, from, null).Result;

            Assert.True(trades.Length > 0);
        }

        [Fact]
        public void GetTradesToDateOnly_Test()
        {
            var pair = "SWTH_NEO";
            var to = new DateTimeOffset(2018, 7, 17, 2, 18, 00, TimeSpan.FromSeconds(0));
            var trades = _repo.GetTrades(pair, null, to).Result;

            Assert.True(trades.Length > 0);
        }

        [Fact]
        public void GetTradesSameDates_Test()
        {
            var pair = "SWTH_NEO";
            var from = new DateTimeOffset(2018, 7, 17, 2, 18, 54, TimeSpan.FromSeconds(0));
            var to = from;
            var trades = _repo.GetTrades(pair, from, to).Result;

            Assert.True(trades.Length == 0);
        }

        [Fact]
        public void GetTradesLimitOnly_Test()
        {
            var pair = "SWTH_NEO";
            var limit = 100;
            var trades = _repo.GetTrades(pair, limit).Result;

            Assert.True(trades.Length == limit);
        }

        [Fact]
        public void GetBalances_Test()
        {
            var balances = _repo.GetBalances(_address).Result;

            Assert.NotNull(balances);
        }

        [Fact]
        public void GetBalancesAuth_Test()
        {
            var balances = _repoAuth.GetBalances().Result;

            Assert.NotNull(balances);
        }

        [Fact]
        public void PostDeposit_Test()
        {
            var asset = "NEO";
            var amount = 3.5M;
            var deposit = _repoAuth.CreateDeposit(asset, amount).Result;

            Assert.NotNull(deposit);
        }

        [Fact]
        public void CreateNeoOrderBuy_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 3;
            var price = .004M;
            var side = Side.buy;
            var order = _repoAuth.CreateNeoOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);
        }

        [Fact]
        public void CreateNeoOrderSell_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 1.2M;
            var price = .004M;
            var side = Side.sell;
            var order = _repoAuth.CreateNeoOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);
        }

        [Fact]
        public void CreateTokenOrderBuy_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 750;
            var price = .004M;
            var side = Side.buy;
            var order = _repoAuth.CreateTokenOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);
        }

        [Fact]
        public void CreateTokenOrderSell_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 100;
            var price = .004M;
            var side = Side.sell;
            var order = _repoAuth.CreateTokenOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);
        }

        [Fact]
        public void CreateAndExecuteNeoOrderBuy_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 3;
            var price = .0004M;
            var side = Side.buy;
            var order = _repoAuth.CreateNeoOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);

            var broadcast = _repoAuth.BroadcastOrder(order).Result;

            Assert.NotNull(broadcast);
        }

        [Fact]
        public void CreateAndExecuteNeoOrderSell_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 1.2M;
            var price = .004M;
            var side = Side.sell;
            var order = _repoAuth.CreateNeoOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);

            var broadcast = _repoAuth.BroadcastOrder(order).Result;

            Assert.NotNull(broadcast);
        }

        [Fact]
        public void CreateAndExecuteTokenOrderBuy_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 750;
            var price = .0004M;
            var side = Side.buy;
            var order = _repoAuth.CreateTokenOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);

            var broadcast = _repoAuth.BroadcastOrder(order).Result;

            Assert.NotNull(broadcast);
        }

        [Fact]
        public void CreateAndExecuteTokenOrderSell_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 100;
            var price = .004M;
            var side = Side.sell;
            var order = _repoAuth.CreateTokenOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);

            var broadcast = _repoAuth.BroadcastOrder(order).Result;

            Assert.NotNull(broadcast);
        }

        [Fact]
        public void CreateAndExecuteNeoOrderSellAndCreateCancelation_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 1.2M;
            var price = .004M;
            var side = Side.sell;
            var order = _repoAuth.CreateNeoOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);

            var executedOrder = _repoAuth.BroadcastOrder(order).Result;

            Assert.NotNull(executedOrder);

            var cancellation = _repoAuth.CreateCancellation(executedOrder).Result;

            Assert.NotNull(cancellation);
        }

        [Fact]
        public void CreateAndExecuteNeoOrderSellAndCreateAndExcecuteCancelation_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 1.2M;
            var price = .004M;
            var side = Side.sell;
            var order = _repoAuth.CreateNeoOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);

            var executedOrder = _repoAuth.BroadcastOrder(order).Result;

            Assert.NotNull(executedOrder);

            var cancellation = _repoAuth.CreateCancellation(executedOrder).Result;

            Assert.NotNull(cancellation);

            var executedCancellation = _repoAuth.ExecuteCancellation(cancellation).Result;

            Assert.NotNull(executedCancellation);
        }

        [Fact]
        public void PlaceNeoOrderSell_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 1.2M;
            var price = .04M;
            var side = Side.sell;
            var order = _repoAuth.PlaceNeoOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);
        }

        [Fact]
        public void PlaceTokenOrderBuy_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 50000;
            var price = .0001M;
            var side = Side.buy;
            var order = _repoAuth.PlaceOrder(pair, side, price, amount, false).Result;

            Assert.NotNull(order);
        }

        [Fact]
        public void CreateDeposit_Test()
        {
            var symbol = "NEO";
            var amount = 1M;

            var deposit = _repoAuth.CreateDeposit(symbol, amount).Result;

            Assert.NotNull(deposit);
        }

        [Fact]
        public void CreateAndExecuteDeposit_Test()
        {
            var symbol = "NEO";
            var amount = 1M;

            var deposit = _repoAuth.CreateDeposit(symbol, amount).Result;

            Assert.NotNull(deposit);

            var executedDeposit = _repoAuth.ExecuteDeposit(deposit).Result;

            Assert.NotNull(executedDeposit);
        }

        [Fact]
        public void Deposit_Test()
        {
            var symbol = "NEO";
            var amount = 1M;

            var deposit = _repoAuth.Deposit(symbol, amount).Result;

            Assert.True(deposit);
        }

        [Fact]
        public void CreateWithdrawal_Test()
        {
            var symbol = "NEO";
            var amount = 1M;

            var withdrawal = _repoAuth.CreateWithdrawal(symbol, amount).Result;

            Assert.NotNull(withdrawal);
        }

        [Fact]
        public void CreateAndExecuteWithdrawal_Test()
        {
            var symbol = "NEO";
            var amount = 1M;

            var withdrawal = _repoAuth.CreateWithdrawal(symbol, amount).Result;

            Assert.NotNull(withdrawal);

            var executedWithdrawal = _repoAuth.ExecuteWithdrawal(withdrawal).Result;

            Assert.NotNull(executedWithdrawal);
        }

        [Fact]
        public void Withdrawal_Test()
        {
            var symbol = "NEO";
            var amount = 1M;

            var deposit = _repoAuth.Withdrawal(symbol, amount).Result;

            Assert.True(deposit);
        }
    }
}
