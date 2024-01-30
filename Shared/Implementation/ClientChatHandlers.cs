using Contracts.Shared;
using System.Net;

namespace Core.Implementation;

public sealed partial class ClientChat
{
    private async Task MessageHandler(Message message, IPEndPoint endPoint)
    {
        _log.Message(message);
        await Task.CompletedTask;
    }

    private async Task UsersCommandHandler(Message response, IPEndPoint endPoint)
    {
        _users = response.UsersList;
        _log.Info($"Registered users: {string.Join(", ", _users.Select(u => u.Name))}");
        await Task.CompletedTask;
    }

    private async Task JoinCommandHandler(Message response, IPEndPoint endPoint)
    {
        if (response.Recipient is not null)
        {
            _log.Info("Join to chat succesful");
            _currentUser = response.Recipient;
        }
        await Task.CompletedTask;
    }
}
