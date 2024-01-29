using Contracts.Shared;
using Core.Abstraction;
using Core.Abstraction.EventArgs;
using Core.UIWrappers;
using Domain;
using Persistence.Contexts;
using System.Net;

namespace Core.Implementation;

public class ServerChat : ChatBase
{
    private readonly HashSet<User> _users = new();
    private readonly User _server = new() { Name = "Server" };
    private readonly ChatContext _context;

    public ServerChat(IMessageProvider messageProvider, ChatContext context, ILog log) : base(messageProvider, log)
    {
        _context = context;
    }

    public override async Task StartAsync()
    {
        _log.Info("Server started");

        await base.StartAsync();
    }


    protected override async Task OnReceivedMessageHandler(ReceivedMessageArgs args)
    {
        _log.Info($"Received message from {args.EndPoint}.");
        switch (args.Message.CommandType)
        {
            case Command.Join:
                await JoinCommandHandler(args.Message, args.EndPoint);
                break;
            case Command.Users:
                await UsersCommandHandler(args.Message, args.EndPoint);
                break;
            case Command.Leave:
                await LeftCommandHandler(args.Message, args.EndPoint);
                break;
            default:
                await MessageHandler(args.Message, args.EndPoint);
                break;
        };
    }

    private async Task SendAllAsync(Message message)
    {
        foreach (var client in _users)
        {
            if (client.IPEndPoint is not null)
                await SendAsync(message, client.IPEndPoint);
        }
    }



    private async Task MessageHandler(Message request, IPEndPoint endPoint)
    {
        _log.Info(request);

        MessageEntity messageEntity = new()
        {
            Content = request.Content,
            SenderId = request.Sender.Id,
            RecipientId = request.Recipient?.Id,
            Time = DateTime.Now,
        };

        _context.Messages.Add(messageEntity);
        await _context.SaveAsync(_cancellationToken);  

        if (request.Recipient is not null)
        {
            await SendAsync(request, endPoint);
            if (request.Recipient is not null)
            {
                var recepientEndPoint = _users.FirstOrDefault(x => x.Id == request.Recipient.Id)?.IPEndPoint;
                if(recepientEndPoint is not null)
                    await SendAsync(request, recepientEndPoint);
            }            
        }
        else
        {
            await SendAllAsync(request);
        }

    }

    private async Task LeftCommandHandler(Message request, IPEndPoint endPoint)
    {
        _users.Remove(request.Sender);
        var userEntity = _context.Users.FirstOrDefault(x => x.Name == request.Sender.Name);
        if (userEntity is not null)
        {
            userEntity.LastOnline = DateTime.Now;
            await _context.SaveAsync(_cancellationToken);
        }
        Message response = new() { Sender = _server, Content = $"{request.Sender.Name} left chat. " };
        await SendAllAsync(response);
    }

    private async Task UsersCommandHandler(Message request, IPEndPoint endPoint)
    {
        await SendAsync(Message.Users(_server, _users), endPoint);
    }



    private async Task JoinCommandHandler(Message request, IPEndPoint endPoint)
    {
        var userEntity = _context.Users.FirstOrDefault(x => x.Name.ToLower().Trim() == request.Sender.Name.ToLower().Trim());

        if (userEntity is null)
        {
            userEntity = new() { Name = request.Sender.Name, LastOnline = DateTime.Now };
            _context.Users.Add(userEntity);
            await _context.SaveAsync(_cancellationToken);
        }

        User user = User.FromDomain(userEntity)!;

        AddUserEndpoint(endPoint, user);

        await SendAsync(Message.Join(_server, user), endPoint);

        await SendAllAsync(Message.Common(_server, null, $"{user.Name} joined to chat."));

        await SendAllAsync(Message.Users(_server, _users));

        await SendUnreadedMessageAsync(endPoint, userEntity.Id, userEntity.LastOnline);
    }

    private void AddUserEndpoint(IPEndPoint endPoint, User user)
    {       
        var _client = _users.FirstOrDefault(x => x.Id == user.Id);
        if(_client is null)
        {
            user.IPEndPoint = endPoint;
            _users.Add(user);
        }
        else
        {
            _client.IPEndPoint = endPoint;
        }
    }

    private async Task SendUnreadedMessageAsync(IPEndPoint endPoint, Guid userId, DateTime last)
    {
        var unreaded = _context
            .Messages
            .Where(m =>
            (m.RecipientId == null || m.RecipientId == userId)
            && m.Time > last).ToList();
        foreach (var message in unreaded)
        {
            await SendAsync(Message.FromDomain(message), endPoint);
        }
    }
}
