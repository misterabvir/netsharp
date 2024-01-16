using App.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace App.Models;

internal static class Messages
{
    internal static class Clients
    {
        public static Message JoinedMessage(string username) => new()
        {
            Text = Command.Join.Name,
            Username = username,
            MessageStatus = MessageStatus.System
        };

        public static Message QuitMessage(string username) => new()
        {
            Text = Command.Exit.Name,
            Username = username,
            MessageStatus = MessageStatus.System
        };
    }

    internal static class Servers
    {
        public static Message JoinedMessage(string username) => new()
        {
            Text = $"{username} {Constants.USER_JOINED}",
            Username = Constants.SERVER_NAME,
            MessageStatus = MessageStatus.System
        };

        public static Message QuitMessage(string username) => new()
        {
            Text = $"{username} {Constants.USER_LEFT}",
            Username = Constants.SERVER_NAME,
            MessageStatus = MessageStatus.System
        };

        public static Message ErrorReadMessage() => new()
        {
            Text = Constants.SERVER_CAN_NOT_READ_MESSAGE,
            Username = Constants.SERVER_NAME,
            MessageStatus = MessageStatus.Error
        };

        internal static Message ShutDown() => new()
        {
            Text = Command.Exit.Name,
            Username = Constants.SERVER_NAME,
            MessageStatus = MessageStatus.System
        };

        internal static Message ShutDownMessage(string username) => new()
        {
            Text = $"{username} {Constants.USER_SHUTDOWN_SERVER}",
            Username = Constants.SERVER_NAME,
            MessageStatus = MessageStatus.System
        };
    }

    internal static class Shared
    {
        public static Message CommonMessage(string username, string text) => new()
        {
            Text = text,
            Username = username,
            MessageStatus = MessageStatus.Common
        };
    }
}