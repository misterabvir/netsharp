using App.Extensions;
using App.Infrastructure;
using App.Models;
using System.Net;
using System.Net.Sockets;
using static App.Models.Messages;

namespace App.Chat;

internal sealed class Server : UdpChat
{
    private Dictionary<string, IPEndPoint> _users = [];


    public override async Task RunAsync()
    {
        using UdpClient client = new(serverPort);
        Log.Information(Constants.SERVER_IS_RUNNING);
        Task.Run(Quit);
        await RunConversation(client);
        client.Close();
    }

    private async Task Quit()
    {
        while (!Command.Exit.Is(await UserInput.ConsoleInput(default))) { }
        _cancellationTokenSource.Cancel();
        
    }


    private async Task RunConversation(UdpClient client)
    {

        while (true)
        {
            try
            {
                await ReceiveAsync(client);
            }
            catch (OperationCanceledException)
            {
                Log.Information(Constants.SERVER_MUST_BE_SHUTDOWN);
                await SendAllAsync(client, Servers.ShutDown());
                return;
            }
            catch (Exception error)
            {
                Log.Error(error.Message.ToString());
            }
        }
    }

    protected override async Task ReceiveAsync(UdpClient client)
    {
        UdpReceiveResult data = await client.ReceiveAsync(_cancellationToken);
        Message? message = data.Buffer.FromBytes();
        if (message is not null)
        {
            await AcceptMessageHandler(client, message, data.RemoteEndPoint);
        }
        else
        {
            Log.Information(Constants.SERVER_CAN_NOT_READ_MESSAGE);
            await SendAsync(client, Servers.ErrorReadMessage(), data.RemoteEndPoint);
        }
    }

    private async Task AcceptMessageHandler(UdpClient client, Message receive, IPEndPoint endPoint)
    {
        Log.Information($"{Constants.USER_RECEIVE_MESSAGE} {receive.Username}");
        if (Command.ShutDown.Is(receive.Text))
        {
            await SendAllAsync(client, Servers.ShutDownMessage(receive.Username));
            _cancellationTokenSource.Cancel();
        }
        else if(Command.Join.Is(receive.Text) && !_users.Values.Contains(endPoint))
        {
            _users[receive.Username] = endPoint;
            Log.Information($"{receive.Username} {Constants.USER_JOINED}");
            await SendAllAsync(client, Servers.JoinedMessage(receive.Username));
        }
        else if (Command.Exit.Is(receive.Text))
        {
            _users.Remove(receive.Username);
            Log.Information($"{receive.Username} {Constants.USER_LEFT}");
            await SendAllAsync(client, Servers.QuitMessage(receive.Username));
        }
        else
        {
            await SendAllAsync(client, receive);
        }
    }

    private async Task SendAllAsync(UdpClient client, Message response)
    {

        foreach (var user in _users.ToArray())
        {
            try
            {
                await SendAsync(client, response, user.Value);
            }
            catch (SocketException)
            {
                Log.Error($"{user.Key} {Constants.CLIENT_DISCONECTED}");
                _users.Remove(user.Key);
            }
        }
    }
}
