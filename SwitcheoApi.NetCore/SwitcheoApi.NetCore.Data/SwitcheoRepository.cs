using SwitcheoApi.NetCore.Core;
using SwitcheoApi.NetCore.Core.Interface;
using SwitcheoApi.NetCore.Data.Interface;
using SwitcheoApi.NetCore.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitcheoApi.NetCore.Data
{
    public class SwitcheoRepository : ISwitcheoRepository
    {
        private string _baseUrl = "https://api.switcheo.network";
        private IRestRepository _restRepo;
        private Helper _helper;

        public SwitcheoRepository()
        {
            _restRepo = new RestRepository();
            _helper = new Helper();
        }

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
        /// <returns>Array of Contracts</returns>
        public async Task<Contract[]> GetContracts()
        {
            var endpoint = "/v2/contracts";

            var url = _baseUrl + endpoint;

            try
            {
                var response = await _restRepo.GetApiStream<Contract[]>(url);

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
            var endpoint = "/v2/tickers/candlesticks";
            if(endTime == 0)
            {
                endTime = _helper.UTCtoUnixTime();
            }
            var startTime = _helper.GetFromUnixTime(endTime, interval, stickCount);
            var queryString = new List<string>
            {
                $"pair={pair}",
                $"interval={interval}",
                $"startTime={startTime}",
                $"endTime={endTime}",
            };

            var url = _baseUrl + endpoint + _helper.ListToString(queryString);

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
        /// <returns>Array LastPrices</returns>
        public async Task<LastPrices[]> GetLastPrice(string[] symbols = null, string[] bases = null)
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
                var response = await _restRepo.GetApiStream<LastPrices[]>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
