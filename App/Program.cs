using Core.Abstraction;
using Core.UIWrappers;
using Core.Implementation;
using Persistence.Contexts;
using System.Net;
using System.Net.Sockets;

IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000);
ILog log = new Log();
IUserInput input = new UserInput(); 
ChatBase chat;
IMessageProvider messageProvider;

if (args.Length != 0)
{
    messageProvider = new MessageProvider(new UdpClient(0));
    chat = new ClientChat(args.First(), messageProvider, serverEndPoint, log, input);   
}
else
{
    ChatContext context = new();
    messageProvider = new MessageProvider(new UdpClient(serverEndPoint));
    chat = new ServerChat(messageProvider, context, log);
}

await chat.StartAsync();