using System.Net;
using Contracts.Shared;

namespace Core.Implementation
{
    public partial class ServerChat
    {

        private async Task MessageHandler(Message request, IPEndPoint endPoint)
        {
            _log.Info(request);

            await _messageRepository.CreateMessage(request.ToDomain(), _cancellationToken);

            if (request.Recipient is not null)
            {
                await SendAsync(request, endPoint);
                if (request.Recipient is not null)
                {
                    var recipientEndPoint = _users.FirstOrDefault(x => x.Id == request.Recipient.Id)?.EndPoint;
                    if (recipientEndPoint is not null)
                    {
                        await SendAsync(request, recipientEndPoint);
                    }
                }
            }
            else
            {
                await SendAllAsync(request);
            }
        }

        private async Task ExitCommandHandler(Message request, IPEndPoint endPoint)
        {
            _users.Remove(request.Sender);
            await _userRepositopry.UpdateUserLastOnline(request.Sender.Id, _cancellationToken);
            Message response = Message.Common(_server, null, $"{request.Sender.Name} left chat. ");
            await SendAllAsync(response);
        }

        private async Task UsersCommandHandler(Message request, IPEndPoint endPoint)
        {
            await SendAsync(Message.Users(_server, _users), endPoint);
        }

        private async Task JoinCommandHandler(Message request, IPEndPoint endPoint)
        {
            User user = User.FromDomain(await _userRepositopry.GetOrCreateUserByName(request.Sender.Name, _cancellationToken))!;
            SetUserEndpoint(user, endPoint);

            await SendAsync(Message.Join(_server, user), endPoint);
            await SendAllAsync(Message.Common(_server, null, $"{user.Name} joined to chat."));
            await SendAllAsync(Message.Users(_server, _users));
            await SendUnreadedMessageAsync(user.Id);
        }
    }
}
