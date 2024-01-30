using Contracts.Shared;
using System.Net;

namespace Core.Abstraction.Services.Events;

public record ReceivedMessageArgs(Message Message, IPEndPoint EndPoint);
