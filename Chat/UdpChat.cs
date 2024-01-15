using HW1.Extensions;
using HW1.Models;
using System.Net;
using System.Net.Sockets;

namespace HW1.Chat;

internal abstract class UdpChat
{
    protected readonly Configuration _config;
    protected readonly CancellationTokenSource _cancellationTokenSource;
    protected readonly CancellationToken _cancellationToken;

    protected UdpChat(Configuration config)
    {
        _config = config;
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
    }
    
    public abstract Task Start();
    protected abstract Task Receive();

    protected async Task Send(Message response)
    {
        using UdpClient sender = new();
        await sender.SendAsync(response.ToBytes(), new IPEndPoint(_config.Address, _config.Remote), _cancellationToken);
    }

    protected abstract Task RunConversation();
}
