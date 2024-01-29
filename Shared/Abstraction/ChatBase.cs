﻿using Contracts.Shared;
using Core.Abstraction.EventArgs;
using Core.UIWrappers;
using System.Net;

namespace Core.Abstraction;

public abstract class ChatBase
{
    protected readonly IMessageProvider _messageProvider;
    protected readonly ILog _log;
    protected CancellationToken _cancellationToken;
    protected CancellationTokenSource _cancellationTokenSource;
    protected bool _showLogSender = false;
    protected ChatBase(IMessageProvider messageProvider, ILog log)
    {
        _cancellationTokenSource = new();
        _cancellationToken = _cancellationTokenSource.Token;
        _log = log;

        _messageProvider = messageProvider;
        _messageProvider.OnErrorExcept += OnErrorExceptHandler;
        _messageProvider.OnSendedMessage += OnSendedMessageHandler;
        _messageProvider.OnReceivedMessage += OnReceivedMessageHandler;
    }


    public virtual async Task StartAsync()
    {
        await _messageProvider.Listener(_cancellationToken);
    }

    protected abstract Task OnReceivedMessageHandler(ReceivedMessageArgs args);

    private async Task OnErrorExceptHandler(ErrorExceptArgs args)
    {
        await Task.CompletedTask;
        _log.Error(args.Message, args.StackTrace);
    }

    private async Task OnSendedMessageHandler(SendedMessageArgs args)
    {
        await Task.CompletedTask;
        if(_showLogSender)
            _log.Info($"Sended {args.CountOfSendedBytes} to {args.EndPoint.Address}:{args.EndPoint.Port}");
    }

    protected async Task SendAsync(Message message, IPEndPoint endPoint)
    {
        await _messageProvider.SendAsync(message, endPoint, _cancellationToken);
    }
}
