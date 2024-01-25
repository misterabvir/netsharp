namespace Core.Domain.Models;

public record class HistoryMessage(string username, string content, DateTime date);