using App.Extensions;
using App.Infrastructure;
using App.Models;
using System.Net;
using System.Net.Sockets;

namespace App.Core;

internal abstract class UdpChat
{
    protected IPAddress serverIP = IPAddress.Parse(Constants.SERVER_ADDRESS);
    protected int serverPort = Constants.SERVER_PORT;
    protected readonly CancellationTokenSource _cancellationTokenSource;
    protected readonly CancellationToken _cancellationToken;

    protected UdpChat()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
    }

    public abstract Task RunAsync();
    
    protected abstract Task ReceiveAsync(UdpClient client);

    protected async Task SendAsync(UdpClient client, Message message, IPEndPoint? endPoint = null)
    {
        byte[] data = message.ToBytes();
        await client.SendAsync(data, data.Length, endPoint);
    }
}
