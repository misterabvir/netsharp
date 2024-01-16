using App.Chat;

UdpChat? _chat = UdpChatFactory.Create(args);
if (_chat is not null)
{
    await _chat.RunAsync();
}