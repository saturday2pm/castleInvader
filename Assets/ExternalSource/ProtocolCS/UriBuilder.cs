using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolCS//.Utility
{
    public class UriBuilder
    {
        public static string Create(string host, string playerId, string accessToken)
        {
            if (host.StartsWith("ws://") == false &&
                host.StartsWith("wss://") == false)
                throw new ArgumentException("{nameof(host)} must start with [ws://, wss://]");

            return "{host}?playerId={playerId}&accessToken={accessToken}&version={Constants.ProtocolVersion.version}";
        }
    }
}
