using System.Net;
using System.Net.Sockets;
using Contracts.Shared;
using Infrastructure.Services.Abstractions;
using Infrastructure.Services.Implementations.EventArgs;

namespace Infrastructure.Services.Implementations;

public sealed class MessageProvider : IMessageProvider
{

    private readonly UdpClient _udpClient;

    public delegate Task ReceivedMessage(ReceivedMessageArgs args);
    public delegate Task SendedMessage(SendedMessageArgs args);
    public delegate Task ErrorExcept(ErrorExceptArgs args);

    public event ReceivedMessage? OnReceivedMessage;
    public event ErrorExcept? OnErrorExcept;
    public event SendedMessage? OnSendedMessage;

    public MessageProvider(UdpClient udpClient)
    {
        _udpClient = udpClient;
        Console.WriteLine(_udpClient.Client.LocalEndPoint);
    }

    public async Task SendAsync(Message message, IPEndPoint endPoint, CancellationToken cancellationToken)
    {
        try
        {
            var count = await _udpClient.SendAsync(message.ToBytes(), endPoint, cancellationToken);
            OnSendedMessage?.Invoke(new SendedMessageArgs(count, endPoint));
        }
        catch (Exception error)
        {
            OnErrorExcept?.Invoke(new ErrorExceptArgs(error.Message, error.StackTrace));
        }
    }

    public async Task Listener(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                UdpReceiveResult data = await _udpClient.ReceiveAsync(cancellationToken);
                Message message = Message.FromBytes(data.Buffer) ?? throw new Exception("not readable data");
                OnReceivedMessage?.Invoke(new ReceivedMessageArgs(message, data.RemoteEndPoint));
            }
            catch (Exception error)
            {
                OnErrorExcept?.Invoke(new ErrorExceptArgs(error.Message, error.StackTrace));
            }
        }
    }
}
