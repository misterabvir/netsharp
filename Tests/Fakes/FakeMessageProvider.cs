using Contracts.Shared;
using Core.Abstraction;
using Core.Implementation;
using System.Net;

namespace Tests.Fakes;

internal class FakeMessageProvider : IMessageProvider
{
    public Message Actual { get; set; }
    public Message Expected { get; set; }

    public event MessageProvider.ReceivedMessage? OnReceivedMessage;
    public event MessageProvider.ErrorExcept? OnErrorExcept;
    public event MessageProvider.SendedMessage? OnSendedMessage;

    public async Task Listener(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        OnReceivedMessage?.Invoke(new Core.Abstraction.EventArgs.ReceivedMessageArgs(Expected, new IPEndPoint(IPAddress.Loopback, 0)));
    }

    public async Task SendAsync(Message message, IPEndPoint endPoint, CancellationToken cancellationToken)
    {
        Actual = message;
        await Task.CompletedTask;
    }
}
