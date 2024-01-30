using Contracts.Shared;
using Core.Abstraction.Common;
using Core.Abstraction.Services;
using Core.Abstraction.Services.Events;
using Persistence.Repositories;
using System.Net;

namespace Core.Implementation;

public partial class ServerChat : ChatBase
{
    private readonly HashSet<User> _users = new();
    private readonly User _server = new() { Name = "Server" };
    private readonly IUserRepository _userRepositopry;
    private readonly IMessageRepository _messageRepository;
    

    public ServerChat(
        IMessageProvider messageProvider, 
        ILog log, 
        IUserRepository userRepositopry,                      
        IMessageRepository messageRepository) 
        : base(messageProvider, log)
    {
        _userRepositopry = userRepositopry;
        _messageRepository = messageRepository;

        _handlers.Add(Command.Join, JoinCommandHandler);
        _handlers.Add(Command.Users, UsersCommandHandler);
        _handlers.Add(Command.Exit, ExitCommandHandler);


    }

    public override async Task StartAsync()
    {
        _log.Info("Server started");

        await base.StartAsync();
    }

    protected override async Task OnReceivedMessageHandler(ReceivedMessageArgs args)
    {
        _log.Info($"Received message from {args.EndPoint}.");
       
        if (_handlers.TryGetValue(args.Message.Command, out Func<Message, IPEndPoint, Task>? execute))
        {
            await execute.Invoke(args.Message, args.EndPoint);
        }
        else
        {
            await MessageHandler(args.Message, args.EndPoint);
        }
    }

    private void SetUserEndpoint(User user, IPEndPoint endPoint)
    {
        var _client = _users.FirstOrDefault(x => x.Id == user.Id);
        if (_client is null)
        {
            user.EndPoint = endPoint;
            _users.Add(user);
        }
        else
        {
            _client.EndPoint = endPoint;
        }
    }

    private async Task SendUnreadedMessageAsync(Guid id)
    {
        var user = _users.First(x => x.Id == id);
        
        var unreaded = (await _messageRepository.GetUnreadedMessages(user.Id, user.LastOnline, _cancellationToken))
            .Select(Message.FromDomain);
        foreach (var message in unreaded)
        {
            await SendAsync(message, user.EndPoint!);
        }
    }

    private async Task SendAllAsync(Message message)
    {
        foreach (var client in _users)
        {
            if (client.EndPoint is not null)
            {
                await SendAsync(message, client.EndPoint);
            }
        }
    }
}

