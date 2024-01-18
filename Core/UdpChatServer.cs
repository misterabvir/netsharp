using App.Extensions;
using App.Infrastructure;
using App.Models;
using System.Net;
using System.Net.Sockets;
using static App.Models.Messages;

namespace App.Core;

internal sealed class UdpChatServer : UdpChat
{
    private readonly Dictionary<string, Func<UdpClient, Message, IPEndPoint, Task>> _executions = [];
    private readonly Dictionary<string, IPEndPoint> _users = [];

    public UdpChatServer()
    {
        _executions.Add(Command.Join.Name, UserJoinHandler);
        _executions.Add(Command.Exit.Name, UserLeftHandler);
        _executions.Add(Command.ShutDown.Name, ShutDownHandler);
    }

    public override async Task RunAsync()
    {
        using UdpClient client = new(serverPort);
        Log.Information(Constants.SERVER_IS_RUNNING);
        Task.Run(StopConversation);
        await RunConversation(client);
        client.Close();
    }

    /// <summary>
    /// Send message to all users
    /// </summary>
    /// <param name="client"> Current instance of <see cref="UdpClient"/></param>
    /// <param name="response"> Message to send </param>
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
                Log.Error($"{user.Key} {Constants.CLIENT_DISCONNECTED}");
                _users.Remove(user.Key);
            }
        }
    }

    /// <summary>
    /// Receive message
    /// </summary>
    /// <param name="client">Current instance of <see cref="UdpClient"/> </param>
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

    /// <summary>
    /// Awaited input on server for stopping
    /// </summary>
    private async Task StopConversation()
    {
        while (!Command.Exit.Is(await UserInput.ConsoleInput(default))) { }
        _cancellationTokenSource.Cancel();

    }

    /// <summary>
    /// Listening incoming messages, and responding
    /// </summary>
    /// <param name="client">Current instance of <see cref="UdpClient"/></param>
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

    /// <summary>
    /// Accepting incoming messages
    /// </summary>
    /// <param name="client">Current instance of <see cref="UdpClient"/></param>
    /// <param name="message"> Received message </param>
    /// <param name="endPoint"> Client IP </param>
    private async Task AcceptMessageHandler(UdpClient client, Message message, IPEndPoint endPoint)
    {
        if (_executions.TryGetValue(message.Text, out Func<UdpClient, Message, IPEndPoint, Task>? execution))
        {
            await execution(client, message, endPoint);
        }
        else
        {
            await SendMessageHandler(client, message, endPoint);
        }
    }

    /// <summary>
    /// Common message sending
    /// </summary>
    /// <param name="client">Current instance of <see cref="UdpClient"/></param>
    /// <param name="message"> Received message </param>
    /// <param name="endPoint"> Client IP </param>
    private async Task SendMessageHandler(UdpClient client, Message message, IPEndPoint endPoint)
    {
        #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        if (_users.TryGetValue(message.Recipient.ToLower(), out endPoint))
        {
            Log.Information(@$"{message.Sender} {Constants.SERVER_SENDED_MESSAGE_FOR_USER} {message.Recipient}");
            await SendAsync(client, message, endPoint);
        }
        else
        {
            Log.Information(@$"{message.Sender} {Constants.SERVER_SENDED_MESSAGE_FOR_ALL}");
            await SendAllAsync(client, message);
        }
        #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }

    /// <summary>
    /// Handling left user
    /// </summary>
    /// <param name="client">Current instance of <see cref="UdpClient"/></param>
    /// <param name="message"> Received message </param>
    /// <param name="endPoint"> Client IP </param>
    private async Task UserLeftHandler(UdpClient client, Message message, IPEndPoint endPoint)
    {
        _users.Remove(message.Sender);
        Log.Information($"{message.Sender} {Constants.USER_LEFT}");
        await SendAllAsync(client, Servers.QuitMessage(message.Sender));
    }

    /// <summary>
    /// Handling joined user
    /// </summary>
    /// <param name="client">Current instance of <see cref="UdpClient"/></param>
    /// <param name="message"> Received message </param>
    /// <param name="endPoint"> Client IP </param>
    private async Task UserJoinHandler(UdpClient client, Message message, IPEndPoint endPoint)
    {
        if (!_users.ContainsKey(message.Sender.ToLower()))
        {
            _users[message.Sender.ToLower()] = endPoint;
            Log.Information($"{message.Sender} {Constants.USER_JOINED}");
            await SendAllAsync(client, Servers.JoinedMessage(message.Sender));
        }
        else
        {
            Log.Information($"{message.Sender} {Constants.USER_ALREADY_JOINED}");
            await SendAsync(client, Servers.AlreadyJoinedMessage(message.Sender), _users[message.Sender.ToLower()]);
        }
    }

    /// <summary>
    /// Stopping server from user command
    /// </summary>
    /// <param name="client">Current instance of <see cref="UdpClient"/></param>
    /// <param name="message"> Received message </param>
    /// <param name="endPoint"> Client IP </param>
    private async Task ShutDownHandler(UdpClient client, Message message, IPEndPoint endPoint)
    {
        await SendAllAsync(client, Servers.TurnOffServer(message.Sender));
        _cancellationTokenSource.Cancel();
    }
}
