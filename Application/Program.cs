using Core;
using System.Net;

IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000);

if (args.Length == 0)
{
    new Server(iPEndPoint);
Console.ReadLine();
}
else
{
    var chat = new Client(args[0], iPEndPoint);
    await chat.Run();
}
