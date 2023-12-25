using HW1.Extensions;
using HW1.Models;
using System.Net;
using System.Net.Sockets;

namespace HW1.Chat;

internal abstract class UdpChat
{
    protected readonly Config _config;

    protected UdpChat(Config config)=> _config = config;
    
    public abstract Task Start();
    protected abstract Task Receive();

    protected async Task Send(Message response)
    {
        using UdpClient sender = new();
        await sender.SendAsync(response.ToBytes(), new IPEndPoint(_config.Address, _config.Remote));
    }
}
