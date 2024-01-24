using App.Core;
using App.Infrastructure;

UdpChat? _chat = UdpChatFactory.Create(
    args, 
    new ConsoleLogger(), 
    new ConsoleInput());
    
if (_chat is not null)
{
    await _chat.RunAsync();
}