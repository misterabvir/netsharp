using Contracts.Shared;
using System.Net;
using Core.Abstraction.Services.Events;


namespace Core.Abstraction.Services;

public interface IMessageProvider
{
    delegate Task ReceivedMessage(ReceivedMessageArgs args);
    delegate Task SendedMessage(SendedMessageArgs args);
    delegate Task ErrorExcept(ErrorExceptArgs args);

    event ReceivedMessage? OnReceivedMessage;
    event ErrorExcept? OnErrorExcept;
    event SendedMessage? OnSendedMessage;

    Task SendAsync(Message message, IPEndPoint endPoint, CancellationToken cancellationToken);
    Task Listener(CancellationToken cancellationToken);
}