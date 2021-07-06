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

            var addresses = new[] {
                "10.0.0.5",
                "172.16.1.2",
                "2001:0db8:0000:0000:0000:ff00:0042:8329",
                "0:0:0:0:0:FFFF:0A00:0005"
            };

            var networktests = new[] {
                "192.168.0.16",
                "192.168.123.123",
                "192.168.1.1",
                "192.168.1.5",
                "2001:0db8:85a3:0000:0000:8a2e:0370:8123",
                "0:0:0:0:0:FFFF:C0A8:0010"
            };

            var addresstests = new[] {
                "10.0.0.5",
                "10.0.0.6",
                "172.16.1.2",
                "192.168.1.5",
                "2001:0db8:0000:0000:0000:ff00:0042:8329",
                "2001:0db8:0000:0000:0000:ff00:0042:8330",
                "0:0:0:0:0:FFFF:0A00:0005"
            };

            using (var ctx = new IPNetworkTestDBContext())
            {
                ctx.ClearNetworks();
                ctx.ClearAddresses();

                // Add networks
                foreach (var n in networks)
                {
                    if (TryParseNetwork(n, out var network))
                        ctx.Networks.Add(network with { Description = $"Network: {n}, First: {network.Prefix}, Last: {network.Last}" });
                }

                // Add addresses
                foreach (var a in addresses)
                {
                    if (IPAddress.TryParse(a, out var ip))
                        ctx.Addresses.Add(new Address { IP = ip, Description = $"IP: {ip}" });
                }

                ctx.SaveChanges();

                // "Run tests" for networks
                foreach (var t in networktests)
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
                    Console.WriteLine(new string('-', 50));
                }


                Console.WriteLine(new string('=', 50));

                // "Run tests" for addresses
                foreach (var t in addresstests)
                {
                    // Search networks
                    var result = ctx.FindAddress(IPAddress.Parse(t));
                    // Any results?
                    if (result != null)
                    {
                        Console.WriteLine($"{t} found: {result.Description} ({result.Id})");
                    }
                    else
                    {
                        Console.WriteLine($"No address found for {t}");
                    }
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
