using HW1.Models;
using HW1.Extensions;
using System.Net.Sockets;
using HW1.Infrastructure;
using System.Net;

namespace HW1.Chat;

internal class Server : UdpChat
{
    private HashSet<IPEndPoint> _clientEndPoints = [];

    public override async Task RunAsync()
    {
        Log.Information("The server is running.");
        Task.Run(() => CancellTask());
        await RunConversation();
        
    }

    private void CancellTask()
    {
        if (Console.ReadKey().Key == ConsoleKey.Escape) 
        {
            Log.Information("The Server must be shut down.");
            _cancellationTokenSource.Cancel();
        }
    }

    protected async Task RunConversation()
    {
        using UdpClient client = new(serverPort);
        while (true)
        {
            try
            {
                await ReceiveAsync(client);              
            }
            catch (OperationCanceledException)
            {               
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
        
        UdpReceiveResult data = await client.ReceiveAsync(_cancellationToken);     
        Message? receive = data.Buffer.FromBytes();
        string response = receive is not null
                ? $"Message has been received from {receive.Username}"
                : $"The server can't make out the message.";
        Log.Information(response);


        if (receive is not null)
        {
            if (!_clientEndPoints.Contains(data.RemoteEndPoint)
                && Command.Join.Is(receive.Text))
            {
                _clientEndPoints.Add(data.RemoteEndPoint);
                Log.Information($"{receive.Username} join.");
                await SendAllAsync(client, Message.JoinedServerMessage(receive.Username));
            }
            else if (Command.Exit.Is(receive.Text))
            {
                _clientEndPoints.Remove(data.RemoteEndPoint);
                Log.Information($"{receive.Username} out.");
                await SendAllAsync(client, Message.QuitServerMessage(receive.Username));
            }
            else
            {
                await SendAllAsync(client, receive);
            }       
        }
        else
        {
            await SendAsync(client, new Message()
            {
                Username = "Server",
                Text = $"The server can't make out the message."
            },
            data.RemoteEndPoint);
        }      
    }

    private async Task SendAllAsync(UdpClient client, Message response)
    {
        foreach (var ep in _clientEndPoints)
        {
            await SendAsync(client, response, ep);
        }
    }
}
