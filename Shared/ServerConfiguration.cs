using System.Net;

namespace Core;

public static class ServerConfiguration
{
    public static string Host { get; set; } = "127.0.0.1";
    public static int Port { get; set; } = 0;

    public static IPEndPoint ServerUrl => new IPEndPoint(IPAddress.Parse(Host), Port);
}
