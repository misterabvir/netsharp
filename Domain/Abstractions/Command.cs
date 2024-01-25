using Domain.Models;

namespace Domain.Abstractions;

public class Command
{
    public const string JOIN = "/join";
    public const string EXIT = "/exit";
    public const string ONLINE = "/online";
    public User? User { get; set; }
    public List<User>? Users { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }


    public static Command Join(string username, User? user = null) => new () {Name=JOIN, Username = username, User = user };
    public static Command Exit(User user) => new () { Name = EXIT, User = user };
    public static Command Online(List<User>? users = null) => new() { Name = ONLINE, Users = users ?? [] };
}
