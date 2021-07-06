using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;

namespace IPNetworkEFCore
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var networks = new[] {
                "192.168.0.0/24",   // IPv4 network
                "192.168.1.1/32",   // IPv4 network, one IP
                "2001:0db8:85a3:0000:0000:8a2e:0370:7334/64", //IPv6 network
                "0:0:0:0:0:FFFF:C0A8:0000/24"       //IPv4 in IPv6 representation network
            };
            
            
            var tests = new[]
{
                "192.168.0.16",
                "192.168.123.123",
                "192.168.1.1",
                "192.168.1.5",
                "2001:0db8:85a3:0000:0000:8a2e:0370:8123",
                "0:0:0:0:0:FFFF:C0A8:0010"
            };

            using (var ctx = new IPNetworkTestDBContext())
            {
                ctx.ClearNetworks();

                // Add networks
                foreach (var n in networks)
                {
                    if (TryParseNetwork(n, out var network))
                        ctx.Networks.Add(network with { Description = $"Network: {n}, First: {network.Prefix}, Last: {network.Last}" });
                }
                ctx.SaveChanges();

                // "Run tests"
                foreach (var t in tests)
                {
                    // Search networks
                    var result = ctx.FindNetworksContaining(IPAddress.Parse(t)).ToArray();
                    // Any results?
                    if (result.Length > 0)
                    {
                        Console.WriteLine($"{t} found in:");
                        foreach (var r in result)
                            Console.WriteLine($"{r.Description} ({r.Id})");
                    }
                    else
                    {
                        Console.WriteLine($"No networks found for {t}");
                    }
                    Console.WriteLine(new string('=', 50));
                }
            }
        }

        private static bool TryParseNetwork(string value, [NotNullWhen(true)] out Network? result)
        {
            var parts = (value ?? string.Empty).Split('/');
            if (parts.Length == 2 && IPAddress.TryParse(parts[0].Trim(), out var prefix) && int.TryParse(parts[1].Trim(), out var prefixlength))
            {
                result = new Network
                {
                    Prefix = prefix,
                    PrefixLength = prefixlength
                };
                return true;
            }
            result = null;
            return false;
        }
    }
}
