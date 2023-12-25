using HW1.Chat;
using System.Net;

IPAddress address = IPAddress.Parse("127.0.0.1");
int serverPort = 10000; 
int clientPort = 12000;


if (args.Length > 0)
{
    UdpChat client = new Client(
        nickName: args[0],
        config: new() { Address = address, Local = clientPort, Remote = serverPort });
    await client.Start();
}
else
{
    UdpChat server = new Server(config: new() { Address = address, Local = serverPort, Remote = clientPort });
    await server.Start();
}
