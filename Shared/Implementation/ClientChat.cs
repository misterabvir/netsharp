﻿using Contracts.Shared;
using Core.Abstraction;
using Infrastructure.Services.Abstractions;
using Infrastructure.Services.Implementations.EventArgs;
using System.Net;

namespace Core.Implementation;

public sealed partial class ClientChat : ChatBase
{

    private readonly IPEndPoint _serverEndPoint;
    private readonly IUserInput _input;
    private IEnumerable<User> _users;
    private User _currentUser;

    public ClientChat(
        string username,
        IMessageProvider messageProvider,
        IPEndPoint endPoint,
        ILog log, 
        IUserInput input)
        : base(messageProvider, log)
    {
        _users = [];
        _currentUser = new User { Name = username };
        _serverEndPoint = endPoint;
        _input = input;
        _handlers.Add(Command.Join, JoinCommandHandler);
        _handlers.Add(Command.Users, UsersCommandHandler);
    }

    public override async Task StartAsync()
    {
        _log.Info("Client started");
        #pragma warning disable CS4014 
        Task.Run(base.StartAsync, _cancellationToken);
        #pragma warning restore CS4014
        await Conversation();
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
                    await SendAsync(Message.Common(_currentUser, recipient, input), _serverEndPoint);
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
        if (_handlers.TryGetValue(args.Message.Command, 
            out Func<Message, IPEndPoint, Task>? execute))
        {
            await execute.Invoke(args.Message, args.EndPoint);
        }
        else
        {
            await MessageHandler(args.Message, args.EndPoint);
        }
    }
}