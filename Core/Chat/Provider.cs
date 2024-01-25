using Core.Extensions;
using Domain.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Core.Chat;

public sealed class Provider
{
    private readonly UdpClient _udpClient;

    public delegate Task ReceiveMessage(Message message, IPEndPoint endPoint);
    public event ReceiveMessage? OnReceiveMessage;


    public Provider(UdpClient udpClient)
    {
        _udpClient = udpClient;
    }


    public async Task SendAsync(Message message, IPEndPoint endPoint)
    {
        try
        {
            await _udpClient.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)), endPoint);
        }
        catch (Exception exc) { await Console.Out.WriteLineAsync("SEND" + exc.Message); }
    }

    public void RunListener(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var data = await _udpClient.ReceiveAsync(cancellationToken);
                    Message? message = data.Buffer.ToMessage();
                    if (message is not null)
                    {
                        OnReceiveMessage?.Invoke(message, data.RemoteEndPoint);
                    }
                }
                catch (Exception exc) { await Console.Out.WriteLineAsync("RECEIVE" + exc.Message); } //TODO Logger
            }
        });
    }

}
