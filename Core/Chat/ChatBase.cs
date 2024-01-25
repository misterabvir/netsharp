using Domain.Models;
using System.Net;
using System.Net.Sockets;

namespace Core.Chat;

public abstract class ChatBase
{
    private readonly Provider _provider;


    protected CancellationToken _cancellationToken;
    protected CancellationTokenSource _cancellationTokenSource;


    protected ChatBase(UdpClient udpClient)
    {
        _cancellationTokenSource = new();
        _cancellationToken = _cancellationTokenSource.Token;

        _provider = new Provider(udpClient);
        _provider.OnReceiveMessage += OnReceiveMessageHandler;
    }

    protected void RunListener() => _provider.RunListener(_cancellationToken);

    protected abstract Task OnReceiveMessageHandler(Message message, IPEndPoint endPoint);

    protected async Task SendMessageAsync(Message message, IPEndPoint endPoint) => await _provider.SendAsync(message, endPoint);
}
