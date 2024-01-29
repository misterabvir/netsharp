using Contracts.Shared;
using System.Net;
using static Core.Implementation.MessageProvider;

namespace Core.Abstraction;

public interface IMessageProvider
{
    event ReceivedMessage? OnReceivedMessage;
    event ErrorExcept? OnErrorExcept;
    event SendedMessage? OnSendedMessage;
    Task SendAsync(Message message, IPEndPoint endPoint, CancellationToken cancellationToken);
    Task Listener(CancellationToken cancellationToken);
}