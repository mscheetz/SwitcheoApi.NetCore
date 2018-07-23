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
        private Helper _helper;

        public SwitcheoRepositoryTests()
        {
            _repo = new SwitcheoRepository(true);
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
    }
}
