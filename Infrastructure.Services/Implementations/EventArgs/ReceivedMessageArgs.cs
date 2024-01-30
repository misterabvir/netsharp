using Contracts.Shared;
using System.Net;

namespace Infrastructure.Services.Implementations.EventArgs;

public record ReceivedMessageArgs(Message Message, IPEndPoint EndPoint);
