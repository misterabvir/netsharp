using System.Net;

namespace Infrastructure.Services.Implementations.EventArgs;

public record SendedMessageArgs(int CountOfSendedBytes, IPEndPoint EndPoint);
