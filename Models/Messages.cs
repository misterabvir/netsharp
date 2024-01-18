using App.Infrastructure;

namespace App.Models;

internal static class Messages
{
    internal static class Clients
    {
        public static Message JoinedMessage(string sender) => new()
        {
            Text = Command.Join.Name,
            Sender = sender,
            Recipient = Constants.SERVER_NAME,
            MessageStatus = MessageStatus.System
        };

        public static Message QuitMessage(string sender) => new()
        {
            Text = Command.Exit.Name,
            Sender = sender,
            Recipient = Constants.SERVER_NAME,
            MessageStatus = MessageStatus.System
        };
    }

    internal static class Servers
    {
        public static Message JoinedMessage(string sender) => new()
        {
            Text = $"{sender} {Constants.USER_JOINED}",
            Sender = Constants.SERVER_NAME,
            Recipient = string.Empty,
            MessageStatus = MessageStatus.System
        };

         public static Message AlreadyJoinedMessage(string recipient) => new()
        {
            Text = $"{recipient} {Constants.USER_ALREADY_JOINED}",
            Sender = Constants.SERVER_NAME,
            Recipient = recipient,
            MessageStatus = MessageStatus.System
        };

        public static Message QuitMessage(string sender) => new()
        {
            Text = $"{sender} {Constants.USER_LEFT}",
            Sender = Constants.SERVER_NAME,
            Recipient = string.Empty,
            MessageStatus = MessageStatus.System
        };

        public static Message ErrorReadMessage() => new()
        {
            Text = Constants.SERVER_CAN_NOT_READ_MESSAGE,
            Sender = Constants.SERVER_NAME,
            Recipient = string.Empty,
            MessageStatus = MessageStatus.Error
        };

        internal static Message ShutDown() => new()
        {
            Text = Command.Exit.Name,
            Sender = Constants.SERVER_NAME,
            Recipient = string.Empty,
            MessageStatus = MessageStatus.System
        };

        internal static Message TurnOffServer(string sender) => new()
        {
            Text = $"{sender} {Constants.USER_TURN_OF_SERVER}",
            Sender = Constants.SERVER_NAME,
            Recipient = string.Empty,
            MessageStatus = MessageStatus.System
        };
    }

    internal static class Shared
    {
        public static Message CommonMessage(string sender, string recipient, string text) => new()
        {
            Text = text,
            Sender = sender,
            Recipient = recipient,
            MessageStatus = MessageStatus.Common
        };
    }
}