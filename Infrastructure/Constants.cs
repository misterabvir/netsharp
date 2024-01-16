namespace App.Infrastructure;

internal static class Constants
{
    public const string USER_JOINED = "has joined the chat";
    public const string USER_LEFT = "left chat";
    public const string USER_RECEIVE_MESSAGE = "Message has been received from";
    public const string USER_SHUTDOWN_SERVER = "user shutdown server.";

    public const string SERVER_NAME = "Server";
    public const string SERVER_CAN_NOT_READ_MESSAGE = "The server can't make out the message.";
    public const string SERVER_MUST_BE_SHUTDOWN = "The Server must be shut down.";
    public const string SERVER_IS_STOPPED = "The Server stopped";
    public const string SERVER_IS_RUNNING= "Server is running";

    public const string CLIENT_STOPPED= "The client stopped";
    public const string CLIENT_IS_RUNNING = "The client is running";
    public const string CLIENT_CAN_NOT_READ_MESSAGE = "The client cannot parse the message from the server.";

    public const int SERVER_PORT = 6050;
    public const string SERVER_ADDRESS = "127.0.0.1";
    public const string CLIENT_DISCONECTED = " was disconected";
}
