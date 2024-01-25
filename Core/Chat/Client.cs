using Core.Extensions;
using Domain.Abstractions;
using Domain.Models;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace Core.Chat;

public sealed class Client : ChatBase
{
    private readonly string _username;
    private readonly IPEndPoint _serverEndPoint;
    private User? _user;
    private List<User> _users = [];
    public Client(string username, IPEndPoint serverEndPoint) : base(GetClient(serverEndPoint))
    {
        _username = username;
        _serverEndPoint = serverEndPoint;
        RunListener();
    }

    private static UdpClient GetClient(IPEndPoint serverEndPoint)
    {
        var udp = new UdpClient(0);
        return udp;
    }

    public async Task Run()
    {
        await SendMessageAsync(Message.CommandMessage(Command.Join(_username)), _serverEndPoint);

        while (!_cancellationToken.IsCancellationRequested)
        {
            string input = Console.ReadLine()!;

            Message message = InputParse(input);

            await SendMessageAsync(message, _serverEndPoint);
        }
    }

    private Message InputParse(string input)
    {
        if (input == Command.EXIT)
        {
            _cancellationTokenSource.Cancel();

            return Message.CommandMessage(Command.Exit(user: _user!));
        }

        if (input == Command.REGISTER)
        {
            return Message.CommandMessage(Command.Register());
        }

        if(input == Command.HISTORY)
        {
            return Message.CommandMessage(Command.History());
        }



        return Message.CommonMessage(input, _user!.Id, _users.FirstOrDefault(u => input.ToLower().StartsWith(u.Name.ToLower()))?.Id);
    }

    protected override async Task OnReceiveMessageHandler(Message message, IPEndPoint endPoint)
    {
        await Task.CompletedTask;
        
        switch (message.MessageType)
        {
            case MessageType.Info: Log.Info(message.Content!); break;
            case MessageType.Message: Log.Message(_users.FirstOrDefault(u => u.Id == message.SenderId)?.Name ?? "Server", message.Content!); break;
            case MessageType.Command:CommandHandler(message.Command!); break;
            case MessageType.Error: break;
        }
    }

    private void CommandHandler(Command command)
    {
        
        
        if (command.Name == Command.EXIT)
        {
            Log.Info("Server is stopped");
            _cancellationTokenSource.Cancel();
            return;
        }

        if (command.Name == Command.JOIN)
        {
            Log.Info($"User {command.User!.Name} joined");
            _user = command.User!;
            return;
        }

        if (command.Name == Command.REGISTER)
        {
            Log.Info($"Registered: [{string.Join(", ", command.Users!.Select(u=>u.Name))}]");
            _users = command.Users!;
            return;
        }

        if (command.Name == Command.HISTORY)
        {
            foreach (var history in command.Messages!)
            {
                Log.Message(history.username, history.content, history.date);
            }
        }
    }
}