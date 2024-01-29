using Contracts.Shared;
using System.Net;

namespace Core.Abstraction.EventArgs;

public record ReceivedMessageArgs(Message Message, IPEndPoint EndPoint);
