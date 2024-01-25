using Core.Domain.Models;
using Domain.Models;

namespace Domain.Abstractions;

public class Command
{
    public const string JOIN = "/join";
    public const string EXIT = "/exit";
    public const string REGISTER = "/register";
    public const string HISTORY = "/history";
    public const string IDENTIFY = "/identify";
    
    public User? User { get; set; }
    public List<User>? Users { get; set; }
    public required string Name { get; set; }
    public string? Username { get; set; }
    public List<HistoryMessage>? Messages { get; set; }

    public static Command Join(string username, User? user = null) => new() { Name = JOIN, Username = username, User = user };
    public static Command Exit(User user) => new() { Name = EXIT, User = user };
    public static Command Register(List<User>? users = null) => new() { Name = REGISTER, Users = users ?? [] };
    public static Command History(List<HistoryMessage>? messages = null) => new() { Name = HISTORY, Messages = messages ?? [] };

}

