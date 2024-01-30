using Contracts.Shared;
using System.Net;
using static Infrastructure.Services.Implementations.MessageProvider;

namespace Infrastructure.Services.Abstractions;

public interface IMessageProvider
{
    event ReceivedMessage? OnReceivedMessage;
    event ErrorExcept? OnErrorExcept;
    event SendedMessage? OnSendedMessage;
    Task SendAsync(Message message, IPEndPoint endPoint, CancellationToken cancellationToken);
    Task Listener(CancellationToken cancellationToken);
}