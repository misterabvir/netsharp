using System.Net;

namespace Core.Abstraction.Services.Events;

public record SendedMessageArgs(int CountOfSendedBytes, IPEndPoint EndPoint);
