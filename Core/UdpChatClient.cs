using App.Models;
using App.Extensions;
using System.Net.Sockets;
using App.Infrastructure;
using System.Net;

namespace App.Core;

internal sealed class UdpChatClient(string username, ILogger logger, IInput input) : UdpChat(logger, input)
{
    private IPEndPoint Remote =>  new (serverIP, serverPort);
    
    public override async Task RunAsync()
    {
        _logger.Information(Constants.CLIENT_IS_RUNNING);
        using UdpClient client = new();
        await RunConversation(client);
        client.Close();
    }

    private async Task RunConversation(UdpClient client)
    {
        
        await SendAsync(client, Messages.Clients.JoinedMessage(username), Remote);
        
        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        Task.Run(() => ReceiveAsync(client));
        #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
     
        while (true)
        {
            try
            {
                _cancellationToken.ThrowIfCancellationRequested();
                string input = await _input.GetInputAsync(_cancellationToken);

                if (Command.Exit.Is(input))
                {
                    await SendAsync(client, Messages.Clients.QuitMessage(username), Remote);
                    await  _cancellationTokenSource.CancelAsync();
                }
                else 
                {                
                    await SendAsync(client, Messages.Shared.CommonMessage(username, input.FirstWord(), input), Remote);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.Information(Constants.CLIENT_STOPPED);
                
                return;
            }
            catch (Exception error)
            {
                _logger.Error(error.ToString());
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
                    _logger.Information(Constants.SERVER_IS_STOPPED);
                    _logger.Information(Constants.CLIENT_STOPPED);
                    Environment.Exit(0);
                }
                else
                {
                    _logger.Message(receive);
                }                              
            }
            else
            {
                _logger.Error(Constants.CLIENT_CAN_NOT_READ_MESSAGE);
            }
        }
    }
}