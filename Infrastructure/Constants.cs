namespace App.Infrastructure;

/// <summary>
/// Constants
/// </summary>
internal static class Constants
{
    #region User Constants
    public const string USER_JOINED = "has joined the chat";
    public const string USER_LEFT = "left chat";
    public const string USER_RECEIVE_MESSAGE = "Message has been received from";
    public const string USER_TURN_OF_SERVER = "user shutdown server.";
    #endregion
    
    #region Server Constants
    public const string SERVER_NAME = "Server";
    public const string SERVER_CAN_NOT_READ_MESSAGE = "The Server can't make out the message.";
    public const string SERVER_MUST_BE_SHUTDOWN = "The Server must be shut down.";
    public const string SERVER_IS_STOPPED = "The Server stopped";
    public const string SERVER_IS_RUNNING= "Server is running";
    public const string SERVER_SENDED_MESSAGE_FOR_USER= "send message for user";
    public const string SERVER_SENDED_MESSAGE_FOR_ALL= "send message for all users";

    #endregion

    #region Client Constants
    public const string CLIENT_STOPPED= "The client stopped";
    public const string CLIENT_IS_RUNNING = "The client is running";
    public const string CLIENT_CAN_NOT_READ_MESSAGE = "The client cannot parse the message from the server.";
    public const string CLIENT_DISCONNECTED = " was disconnected";
    public const string USER_ALREADY_JOINED = "user already joined the chat";
    #endregion

    #region Server Config Constants    
    public const int SERVER_PORT = 6050;
    public const string SERVER_ADDRESS = "127.0.0.1";
    #endregion
}
