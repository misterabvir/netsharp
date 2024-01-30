using Contracts.Shared;

namespace Core.Abstraction.Services;

public interface ILog
{
    void Error(string message, string? stackTrace);
    void Info(string text);
    void Info(Message message);
    void Message(Message message);
}
