using CommandLine;

namespace App.Infrastructure;

/// <summary>
/// Command line options fo parse args
/// </summary>
internal class Options
{
    [Option('m', "mode", Required = true, HelpText = "Specify mode of operation: server or client")]
    public OperationMode? Mode { get; set; }

    [Option('u', "username", HelpText = "Specify username (required for client mode)")]
    public string? Username { get; set; }
}
