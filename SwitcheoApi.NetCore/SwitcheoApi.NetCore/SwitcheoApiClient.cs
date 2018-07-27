using SwitcheoApi.NetCore.Data;
using SwitcheoApi.NetCore.Data.Interface;

namespace Switcheo.NetCore
{
    public class SwitcheoApiClient
    {
        public ISwitcheoRepository SwitcheoRepository;

        /// <summary>
        /// Constructor, no authorization
        /// </summary>
        /// <param name="testRegion">Boolean to use test region (default = false)</param>
        public SwitcheoApiClient(bool testRegion = false)
        {
            SwitcheoRepository = new SwitcheoRepository(testRegion);
        }

        /// <summary>
        /// Constructor, with authorization
        /// </summary>
        /// <param name="address">Neo address</param>
        /// <param name="privateKey">Neo address private key</param>
        /// <param name="testRegion">Boolean to use test region (default = false)</param>
        public SwitcheoApiClient(string address, string privateKey, bool testRegion = false)
        {
            SwitcheoRepository = new SwitcheoRepository(address, privateKey, testRegion);
        }
    }
}
