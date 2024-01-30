using Contracts.Shared;

namespace Infrastructure.Services.Abstractions;

public interface ILog
{
    void Error(string message, string? stackTrace);
    void Info(string text);
    void Info(Message message);
    void Message(Message message);
}
