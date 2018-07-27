using SwitcheoApi.NetCore.Entities;
using System;
using System.Collections.Generic;

namespace SwitcheoApi.NetCore.Core
{
    public class Helper
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly long epochTicks = new DateTime(1970, 1, 1, 0, 0, 0).Ticks;

        /// <summary>
        /// Convert a list of strings to a single string
        /// </summary>
        /// <param name="list">List of strings to convert</param>
        /// <returns>String from list</returns>
        public string ListToString(List<string> list)
        {
            return string.Join(",", list.ToArray());
        }

        /// <summary>
        /// Convert a list of strings to a query string
        /// </summary>
        /// <param name="list">List of strings to convert</param>
        /// <returns>Query string from list</returns>
        public string ListToQueryString(List<string> list)
        {
            return string.Join("&", list.ToArray());
        }

        /// <summary>
        /// Convert end of current minute to unix timestamp
        /// </summary>
        /// <returns>unix timestamp</returns>
        public long UTCEndOfMinuteToUnixTime()
        {
            var roundedTime = RoundUp(DateTime.UtcNow, TimeSpan.FromMinutes(1));

            return UTCtoUnixTime(roundedTime);
        }

        /// <summary>
        /// Round time up
        /// </summary>
        /// <param name="dateTime">Current time</param>
        /// <param name="timeSpan">Time span to round up to</param>
        /// <returns>Rouned up time</returns>
        public DateTime RoundUp(DateTime dateTime, TimeSpan timeSpan)
        {
            return new DateTime((dateTime.Ticks + timeSpan.Ticks - 1) / timeSpan.Ticks * timeSpan.Ticks, dateTime.Kind);
        }

        /// <summary>
        /// Convert current UTC DateTime to unix timestamp
        /// </summary>
        /// <returns>unix timestamp</returns>
        public long UTCtoUnixTime()
        {
            return UTCtoUnixTime(DateTime.UtcNow);
        }

        /// <summary>
        /// Convert UTC DateTime to unix timestamp
        /// </summary>
        /// <param name="localTime">UTC DateTime object</param>
        /// <returns>unix timestamp</returns>
        public long UTCtoUnixTime(DateTime utcTimestamp)
        {
            return ((utcTimestamp.Ticks - epochTicks) / TimeSpan.TicksPerSecond);
        }

        /// <summary>
        /// Convert UTC DateTime to unix timestamp
        /// </summary>
        /// <param name="localTime">UTC DateTime object</param>
        /// <returns>unix timestamp</returns>
        public long UTCtoUnixTime(DateTimeOffset utcTimestamp)
        {
            return ((utcTimestamp.Ticks - epochTicks) / TimeSpan.TicksPerSecond);
        }

        /// <summary>
        /// Convert current UTC DateTime to unix timestamp milliseconds included
        /// </summary>
        /// <returns>unix timestamp</returns>
        public long UTCtoUnixTimeMilliseconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Get unix time off-set from current unix time
        /// </summary>
        /// <param name="ending">Ending time</param>
        /// <param name="interval">Stick interval</param>
        /// <param name="stickNumber">Number of sticks</param>
        /// <returns>Long of off-set time</returns>
        public long GetFromUnixTime(long ending, Interval interval, int stickNumber)
        {
            var seconds = IntervalToSeconds(interval);
            var totalSeconds = seconds * stickNumber;

            return ending - totalSeconds;
        }

        /// <summary>
        /// Convert interval to total seconds
        /// </summary>
        /// <param name="interval">Interval value</param>
        /// <returns>Long of seconds</returns>
        public long IntervalToSeconds(Interval interval)
        {
            switch (interval)
            {
                case Interval.OneM:
                    return (60 * 1);
                case Interval.FiveM:
                    return (60 * 5);
                case Interval.ThirtyM:
                    return (60 * 30);
                case Interval.OneH:
                    return (60 * 60);
                case Interval.SixH:
                    return (60 * 60 * 6);
                case Interval.OneD:
                    return (60 * 60 * 24);
                default:
                    return (60 * 1);
            }
        }

        /// <summary>
        /// Get supported assets with ids
        /// </summary>
        /// <returns>Dictionary of assets and ids</returns>
        public Dictionary<string, string> SupportedAssets()
        {
            var assets = new Dictionary<string, string>();
            assets.Add("NEO", "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
            assets.Add("GAS", "602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
            assets.Add("SWTH", "ab38352559b8b203bde5fddfa0b07d8b2525e132");
            assets.Add("ACAT", "7f86d61ff377f1b12e589a5907152b57e2ad9a7a");
            assets.Add("APH", "a0777c3ce2b169d4a23bcba4565e3225a0122d95");
            assets.Add("AVA", "de2ed49b691e76754c20fe619d891b78ef58e537");
            assets.Add("CPX", "45d493a6f73fa5f404244a5fb8472fc014ca5885");
            assets.Add("DBC", "b951ecbbc5fe37a9c280a76cb0ce0014827294cf");
            assets.Add("EFX", "acbc532904b6b51b5ea6d19b803d78af70e7e6f9");
            assets.Add("GALA", "9577c3f972d769220d69d1c4ddbd617c44d067aa");
            assets.Add("LRN", "06fa8be9b6609d963e8fc63977b9f8dc5f10895f");
            assets.Add("MCT", "a87cc2a513f5d8b4a42432343687c2127c60bc3f");
            assets.Add("NKN", "c36aee199dbba6c3f439983657558cfb67629599");
            assets.Add("OBT", "0e86a40588f715fcaf7acd1812d50af478e6e917");
            assets.Add("ONT", "ceab719b8baa2310f232ee0d277c061704541cfb");
            assets.Add("PKC", "af7c7328eee5a275a3bcaee2bf0cf662b5e739be");
            assets.Add("RHT", "2328008e6f6c7bd157a342e789389eb034d9cbc4");
            assets.Add("RPX", "ecc6b20d3ccac1ee9ef109af5a7cdb85706b1df9");
            assets.Add("TKY", "132947096727c84c7f9e076c90f08fec3bc17f18");
            assets.Add("TNC", "08e8c4400f1af2c20c28e0018f29535eb85d15b6");
            assets.Add("TOLL", "78fd589f7894bf9642b4a573ec0e6957dfd84c48");
            assets.Add("QLC", "0d821bd7b6d53f5c2b40e217c6defc8bbe896cf5");
            assets.Add("SOUL", "ed07cffad18f1308db51920d99a2af60ac66a7b3");
            assets.Add("ZPT", "ac116d4b8d4ca55e6b6d4ecce2192039b51cccc5");
            assets.Add("SWH", "78e6d16b914fe15bc16150aeb11d0c2a8e532bdd");

            return assets;
        }
    }
}
