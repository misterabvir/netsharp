using App.Models;
using App.Extensions;
using System.Net.Sockets;
using App.Infrastructure;

namespace App.Chat;

internal sealed class Client(string username) : UdpChat
{
    public override async Task RunAsync()
    {
        Log.Information(Constants.CLIENT_IS_RUNNING);
        using UdpClient client = new();
        client.Connect(serverIP, serverPort);
        await RunConversation(client);
    }

    private async Task RunConversation(UdpClient client)
    {
        
        await SendAsync(client, Messages.Clients.JoinedMessage(username));
        
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
                    await SendAsync(client, Messages.Clients.QuitMessage(username));
                    _cancellationTokenSource.Cancel();
                }
                else 
                {                
                    await SendAsync(client, Messages.Shared.CommonMessage(username, input));
                }
            }
            catch (OperationCanceledException)
            {
                Log.Information(Constants.CLIENT_STOPPED);
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
                if (Command.Exit.Is(receive.Text))
                {
                    Log.Information(Constants.SERVER_IS_STOPPED);
                    Log.Information(Constants.CLIENT_STOPPED);
                    Environment.Exit(0);
                }
                else
                {
                    Log.Message(receive);
                }                              
            }
            else
            {
                Log.Error(Constants.CLIENT_CAN_NOT_READ_MESSAGE);
            }
        }
    }

 
}