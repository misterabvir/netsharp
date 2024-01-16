using HW1.Extensions;
using HW1.Models;
using System.Net;
using System.Net.Sockets;

namespace HW1.Chat;

internal abstract class UdpChat
{
    protected IPAddress serverIP = IPAddress.Parse("127.0.0.1");
    protected int serverPort = 12345;

    protected readonly CancellationTokenSource _cancellationTokenSource;
    protected readonly CancellationToken _cancellationToken;

    protected UdpChat()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
    }
    
    public abstract Task RunAsync();
    protected abstract Task ReceiveAsync(UdpClient client);

    protected async Task SendAsync(UdpClient client, Message response, IPEndPoint? ep = null) 
    {
        byte[] data = response.ToBytes();
        await client.SendAsync(data, data.Length, ep);
    }
}
