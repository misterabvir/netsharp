using HW1.Models;
using HW1.Extensions;
using System.Net.Sockets;
using HW1.Infrastructure;

namespace HW1.Chat;

internal class Client(string username, Configuration config) : UdpChat(config)
{
    public override async Task Start()
    {
        Log.Information("The client is up.");
        await RunConversation();
    }

    protected override async Task RunConversation()
    {       
        while (true)
        {
            try
            {
                _cancellationToken.ThrowIfCancellationRequested();
                string input = await UserInput.ConsoleInput(_cancellationToken);
                await Send(new() { Username = username, Text = input });
                if (Command.Exit.Is(input))
                {
                    await _cancellationTokenSource.CancelAsync();
                }
                await Receive();

            }
            catch (OperationCanceledException)
            {
                Log.Information("The client shut down.");
                return;
            }
            catch (Exception error)
            {
                Log.Error(error.ToString());
            }
        }
    }

    protected override async Task Receive()
    {
        using UdpClient receiver = new(_config.Local);
        UdpReceiveResult data = await receiver.ReceiveAsync(_cancellationToken);
        Message? receive = data.Buffer.FromBytes();        
        if (receive is not null)
        {
            receive.Print();
        }
        else
        {
            Log.Error($"The client can't make out the message from server.");
        }
    }
}