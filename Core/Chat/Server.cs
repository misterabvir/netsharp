using Core.Domain.Models;
using Core.Persistence;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using Command = Domain.Abstractions.Command;

namespace Core.Chat;

public sealed class Server : ChatBase
{
    private readonly ChatDbContext _context = new ChatDbContext();
    private Dictionary<Guid, IPEndPoint> _onlineUsers = [];


    public Server(IPEndPoint serverEndPoint) : base(new UdpClient(serverEndPoint))
    {
        RunListener();
        Console.WriteLine("Server is started");
    }

    protected override async Task OnReceiveMessageHandler(Message message, IPEndPoint endPoint)
    {
        Log.Info($"Received message from {endPoint.Address.ToString()}:{endPoint.Port.ToString()}");

        switch (message.MessageType)
        {
            case MessageType.Command: await CommandHandler(message, endPoint); break;
            case MessageType.Message: await MessageHandler(message, endPoint); break;
            default: break;
        }
    }

    private async Task MessageHandler(Message message, IPEndPoint endPoint)
    {
        await _context.Messages.AddAsync(message, _cancellationToken);
        await _context.SaveChangesAsync(_cancellationToken);

        if (message.SenderId is not null)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Id == message.SenderId);

            Log.Message(user?.Name ?? "null", message.Content ?? "null");
        }

        if (message.RecipientId is not null)
        {
            if (_onlineUsers.TryGetValue(message.RecipientId.Value, out IPEndPoint? recipientEndPoint))
            {
                await SendMessageAsync(message, recipientEndPoint);
                await SendMessageAsync(message, endPoint);
              
                Log.Info($"Message sended to {recipientEndPoint.Port}");
            }
            return;
        }

        Log.Info($"Users online [{string.Join(", ", _onlineUsers.Values.Select(u => u.Port))}]");

        foreach (var ep in _onlineUsers.Values)
        {
            await SendMessageAsync(message, ep);
            Log.Info($"Message sended to {ep.Port}");
        }
    }


    private async Task CommandHandler(Message message, IPEndPoint endPoint)
    {
        Log.Info($"Command \'{message.Command!.Name!}\' handling");

        if (message.Command!.Name!.Equals(Command.EXIT))
        {
            await ExitCommandHandler(message);
            return;
        }

        if (message.Command!.Name!.Equals(Command.JOIN))
        {
            await JoinCommandHandler(message, endPoint);
            return;
        }

        if (message.Command!.Name!.Equals(Command.REGISTER))
        {
            await RegisterCommandHandler();
            return;
        }

        if (message.Command!.Name!.Equals(Command.HISTORY))
        {
            await HistoryCommandHandler(endPoint);
        }
    }

    private async Task ExitCommandHandler(Message message)
    {
        _onlineUsers.Remove(message.Command!.User!.Id);
        var user = _context.Users.First(u => u.Id == message.Command!.User!.Id);
        await SendMessageAllAsync(Message.InfoMessage($"{user.Name} left chat"));
        await RegisterCommandHandler();
    } 

    private async Task JoinCommandHandler(Message message, IPEndPoint endPoint)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == message.Command!.Username!.ToLower());
        if (user is null)
        {
            user = new User() { Name = message.Command!.Username! };
            await _context.Users.AddAsync(user, _cancellationToken);
            await _context.SaveChangesAsync(_cancellationToken);
        }
        _onlineUsers[user.Id] = endPoint;

        await SendMessageAsync(Message.CommandMessage(Command.Join(message.Command!.Username!, user)), endPoint);
        
        await SendMessageAllAsync(Message.InfoMessage($"{user.Name} joined to chat"));
        await RegisterCommandHandler();
        await HistoryCommandHandler(endPoint);
    }


    private async Task HistoryCommandHandler(IPEndPoint endPoint)
    {

        var sender = _onlineUsers.FirstOrDefault(o => o.Value.Port == endPoint.Port);

        List<HistoryMessage> messages = [];
        await _context.Messages.ForEachAsync(m =>
        {

            if (m.RecipientId == null || m.RecipientId.Value == sender.Key || m.SenderId == sender.Key)
            {
                string username = _context.Users.First(u => u.Id == m.SenderId).Name;
                string content = m.Content!;
                DateTime date = m.DateTime;
                messages.Add(new HistoryMessage(username, content, date));
            }
        });
        await SendMessageAsync(Message.CommandMessage(Command.History(messages)), endPoint);

    }

    private async Task RegisterCommandHandler()
    {
        var users = _context.Users.ToList();
        await SendMessageAllAsync(Message.CommandMessage(Command.Register(users)));
    }

    private async Task SendMessageAllAsync(Message message)
    {
        foreach (var ep in _onlineUsers.Values) { await SendMessageAsync(message, ep); }
    }
}