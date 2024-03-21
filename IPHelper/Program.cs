using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

var interfaces = NetworkInterface.GetAllNetworkInterfaces();
foreach (var i in interfaces)
{
    Console.WriteLine($"{i.Name} ({i.NetworkInterfaceType})");
    var ipProperties = i.GetIPProperties();
    var ips = ipProperties.UnicastAddresses;
    foreach (var ip in ips)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (ip.PrefixOrigin == PrefixOrigin.RouterAdvertisement && ip.SuffixOrigin == SuffixOrigin.LinkLayerAddress)
                Console.WriteLine($"Main IPv6: {ip.Address} ({(ip.Address.IsIPv6Teredo ? "Teredo" : "Not Teredo")}) ({(IsPrivate(ip.Address) ? "Private" : "Public")})");
            if (ip.PrefixOrigin == PrefixOrigin.Dhcp && ip.Address.AddressFamily == AddressFamily.InterNetwork)
                Console.WriteLine($"IPv4 from DHCP: {ip.Address} ({(IsPrivate(ip.Address) ? "Private" : "Public")})");
        }
        else
        {
            if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                Console.WriteLine($"IPv6: {ip.Address} ({(ip.Address.IsIPv6Teredo ? "Teredo" : "Not Teredo")}) ({(IsPrivate(ip.Address) ? "Private" : "Public")})");
            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                Console.WriteLine($"IPv4: {ip.Address} ({(IsPrivate(ip.Address) ? "Private" : "Public")})");
        }
    }
    Console.WriteLine();
}

static bool IsPrivate(IPAddress ip)
{
    var ipBytes = ip.GetAddressBytes();

    return ip.AddressFamily switch
    {
        AddressFamily.InterNetworkV6 => ip.IsIPv6LinkLocal
                                        || ip == IPAddress.IPv6Loopback
                                        || ip.GetAddressBytes()[0] == 0xFC
                                        || ip.GetAddressBytes()[0] == 0xFD,
        AddressFamily.InterNetwork => (ipBytes[0]) switch
        {
            10 or 127 => true,
            169 => ipBytes[1] == 254,
            172 => ipBytes[1] >= 16 && ipBytes[1] < 32,
            192 => ipBytes[1] == 168,
            _ => false,
        },
        _ => throw new NotImplementedException(),
    };
}
