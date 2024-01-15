using System.Net;

namespace HW1.Models;

internal class Configuration
{
    public required int Remote { get; set; }
    public required int Local { get; set; }
    public required IPAddress Address { get; set; }
}
