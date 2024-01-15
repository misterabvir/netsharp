using HW1.Models;
using HW1.Extensions;
using System.Net.Sockets;
using HW1.Infrastructure;

namespace HW1.Chat;

internal class Server(Configuration config) 
    : UdpChat(config)
{
  
    public override async Task Start()
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

    protected override async Task RunConversation()
    {
        while (true)
        {
            try
            {
                var (receive, response) = await Receive();
                await Send(new() { Username = "Server", Text = response });
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

    protected override async Task<(Message? receive, string response)> Receive()
    {
        using UdpClient receiver = new(_config.Local);
        UdpReceiveResult data = await receiver.ReceiveAsync(_cancellationToken);     
        Message? receive = data.Buffer.FromBytes();
        string response = receive is not null
                ? $"Message has been received from {receive.Username}"
                : $"The server can't make out the message.";
        Log.Information(response);
        return (receive, response);
    }
}
