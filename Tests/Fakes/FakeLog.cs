using Contracts.Shared;
using Core.UIWrappers;

namespace Tests.Fakes;

internal class FakeLog : ILog
{
    public Message Actual { get; private set; } = new Message();
    
    public void Error(string message, string? stackTrace)
    {
        
    }

    public void Info(string message)
    {

    }

    public void Info(Message message)
    {
        Actual = message;
    }

    public void Message(Message message)
    {
        Actual = message;
    }
}
