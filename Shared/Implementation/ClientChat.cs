using Contracts.Shared;
using Core.Abstraction;
using Core.Abstraction.EventArgs;
using Core.UIWrappers;
using System.Net;

namespace Core.Implementation;

public sealed class ClientChat : ChatBase
{

    private readonly IPEndPoint _serverEndPoint;
    private readonly IUserInput _input;
    private IEnumerable<User> _users;
    private User _currentUser;

    public ClientChat(
        string username,
        IMessageProvider messageProvider,
        IPEndPoint endPoint,
        ILog log, IUserInput input)
        : base(messageProvider, log)
    {
        _users = [];
        _currentUser = new User { Name = username };
        _serverEndPoint = endPoint;
        _input = input;
        Task.Run(Conversation);
    }

    public override async Task StartAsync()
    {
        _log.Info("Client started");
        await base.StartAsync();
    }

    private async Task Conversation()
    {
        await SendAsync(Message.Join(_currentUser), _serverEndPoint);
        while (!_cancellationToken.IsCancellationRequested)
        {
            string input = (await _input.ReadLineAsync()) ?? string.Empty;

            switch (input)
            {
                case "/users":
                    await SendAsync(Message.Users(_currentUser, []), _serverEndPoint);
                    break;
                case "/exit":
                    await SendAsync(Message.Leave(_currentUser), _serverEndPoint);
                    await StopAsync();
                    break;
                case "/help":
                    _log.Info("Available commands: /users, /exit, /help");
                    break;
                default:
                    var recipient = _users.FirstOrDefault(u => u != _currentUser && input.StartsWith(u.Name));
                    Message message = new() { Content = input, Recipient = recipient, Sender = _currentUser };
                    await SendAsync(message, _serverEndPoint);
                    break;
            }
        }
    }

    private async Task StopAsync()
    {
        await _cancellationTokenSource.CancelAsync();
    }

    protected override async Task OnReceivedMessageHandler(ReceivedMessageArgs args)
    {
        switch (args.Message.CommandType)
        {
            case Command.Join:
                await JoinCommandHandler(args.Message);
                break;
            case Command.Users:
                await UsersCommandHandler(args.Message);
                break;
            default:
                await MessageHandler(args.Message);
                break;
        }
    }

    private async Task MessageHandler(Message message)
    {
        _log.Message(message);
        await Task.CompletedTask;
    }

    private async Task UsersCommandHandler(Message response)
    {
        _users = response.UsersList;
        _log.Info($"Registered users: {string.Join(", ", _users.Select(u => u.Name))}");
        await Task.CompletedTask;
    }

    private async Task JoinCommandHandler(Message response)
    {
        if (response.Recipient is not null)
        {
            _log.Info("Join to chat succesful");
            _currentUser = response.Recipient;
        }
        await Task.CompletedTask;
    }
}