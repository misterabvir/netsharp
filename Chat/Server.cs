using HW1.Models;
using HW1.Extensions;
using System.Net.Sockets;
using HW1.Infrastructure;

namespace HW1.Chat;

internal class Server(Config config) 
    : UdpChat(config)
{
  
    public override async Task Start()
    {
        Text.Information("The server is running.");

        while (true)
        {
            try
            {
                var (receive, response) = await Receive();
                if (Command.Exit.Is(receive?.Text))
                {
                    Text.Information("The Server shut down.");
                    return;
                }
                await Send(new() { NickName = "Server", Text = response });
            }
            catch (Exception error)
            {
                Text.Error(error.ToString());
            }
        }
    }

    protected override async Task<(Message? receive, string response)> Receive()
    {
        using UdpClient receiver = new(_config.Local);
        UdpReceiveResult data = await receiver.ReceiveAsync();     
        Message? receive = data.Buffer.FromBytes();
        string response = receive is not null
                ? $"Message has been received from {receive.NickName}"
                : $"The server can't make out the message.";
        Text.Information(response);
        return (receive, response);
    }
}
