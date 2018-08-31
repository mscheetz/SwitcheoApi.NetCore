using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Tests
{
    public class TestObjects
    {
        // Empty neo wallet for testing purposes only
        private string _address = "AGA7VMVRpRDULskJ7sWsUt9YuhVj6CHz8y";
        private string _privateKey = "L3SDs1rP2Fs489VGFY4Lt2NAg3Km1PqJsBkQd4QsN8UvotGif1yZ";
        private string _scriptHash = "0x3161dab9941504e080db38f56ed9c722c7b43404";
        
        public string GetAddress()
        {
            return _address;
        }

        public string GetScriptHash()
        {
            return _scriptHash;
        }

        public string GetWIF()
        {
            return _privateKey;
        }
    }
}
