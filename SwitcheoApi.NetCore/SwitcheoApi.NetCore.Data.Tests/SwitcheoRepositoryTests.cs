using SwitcheoApi.NetCore.Core;
using SwitcheoApi.NetCore.Data.Interface;
using SwitcheoApi.NetCore.Entities;
using System;
using Xunit;

namespace SwitcheoApi.NetCore.Data.Tests
{
    public class SwitcheoRepositoryTests
    {
        private ISwitcheoRepository _repo;
        private ISwitcheoRepository _repoAuth;
        private Helper _helper;
        private string _documentationAddress = "87cf67daa0c1e9b6caa1443cf5555b09cb3f8e5f";
        // Empty neo wallet for testing purposes only
        private string _address = "AGA7VMVRpRDULskJ7sWsUt9YuhVj6CHz8y";
        private string _privateKey = "L3SDs1rP2Fs489VGFY4Lt2NAg3Km1PqJsBkQd4QsN8UvotGif1yZ";

        public SwitcheoRepositoryTests()
        {
            _repo = new SwitcheoRepository(true);
            _repoAuth = new SwitcheoRepository(_address, _privateKey, true);
            _helper = new Helper();
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
            var endTime = _helper.UTCtoUnixTime();
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
            var endTime = _helper.UTCtoUnixTime(now);
            var tenDays = now.AddDays(-10);
            var startTime = _helper.UTCtoUnixTime(tenDays);
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
            var address = _documentationAddress;
            var balances = _repo.GetBalances(address).Result;

            Assert.NotNull(balances);
            Assert.True(balances.confirmed.Count > 0);
            Assert.True(balances.confirming.Count > 0);
            Assert.True(balances.locked.Count > 0);
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
        public void CreateOrder_Test()
        {
            var pair = "SWTH_NEO";
            var amount = 1000;
            var price = .0001M;
            var side = Side.buy;
            var order = _repoAuth.CreateOrder(pair, side, price, amount);

            Assert.NotNull(order);
        }
    }
}
