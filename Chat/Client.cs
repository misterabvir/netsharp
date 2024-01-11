using HW1.Models;
using HW1.Extensions;
using System.Net.Sockets;
using HW1.Infrastructure;

namespace HW1.Chat;

internal class Client(string nickName, Config config) : UdpChat(config)
{
    public override async Task Start()
    {
        Text.Information("The client is up.");

        while (true)
        {
            try
            {
                string input = Text.Input();
                await Send(new() { NickName = nickName, Text = input });
                if (Command.Exit.Is(input))
                {
                    Text.Information("The client shut down.");
                    return;
                }
                await Receive();

            }
            catch (Exception error)
            {
                Text.Error(error.ToString());
            }
        }
    }

    protected override async Task Receive()
    {
        using UdpClient receiver = new(_config.Local);
        UdpReceiveResult data = await receiver.ReceiveAsync();
        Message? receive = data.Buffer.FromBytes();        
        if (receive is not null)
        {
            receive.Print();
        }
        else
        {
            Text.Error($"The client can't make out the message from server.");
        }
    }
}