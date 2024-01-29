using System.Net;

namespace Core.Abstraction.EventArgs;

public record SendedMessageArgs(int CountOfSendedBytes, IPEndPoint EndPoint);
