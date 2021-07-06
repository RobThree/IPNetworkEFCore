using System.Linq;
using System.Net;

namespace IPNetworkEFCore
{
    public static class IPAddressExtensionMethods
    {
        public static string AsHex(this IPAddress ip)
            => string.Join(string.Empty, ip.GetAddressBytes().Select(b => b.ToString("X2")));
    }
}
