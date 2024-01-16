using HW1.Models;
using HW1.Extensions;
using System.Net.Sockets;
using HW1.Infrastructure;
using System.Net;

namespace HW1.Chat;

internal class Client(string username) : UdpChat
{
    public override async Task RunAsync()
    {
        Log.Information("The client is up.");
        await RunConversation();
    }

    protected async Task RunConversation()
    {
        using UdpClient client = new();
        client.Connect(serverIP, serverPort);
        await SendAsync(client, Message.JoinedClientMessage(username));
        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        Task.Run(() => ReceiveAsync(client));
        #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

      
        while (true)
        {
            try
            {
                _cancellationToken.ThrowIfCancellationRequested();
                string input = await UserInput.ConsoleInput(_cancellationToken);
                if (Command.Exit.Is(input))
                {
                    await _cancellationTokenSource.CancelAsync();
                    await SendAsync(client, Message.QuitClientMessage(username));
                }
                else 
                {                
                    await SendAsync(client, Message.Create(username, input));
                }
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

    protected override async Task ReceiveAsync(UdpClient client)
    {
        while (true)
        {
            UdpReceiveResult data = await client.ReceiveAsync(_cancellationToken);
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

 
}