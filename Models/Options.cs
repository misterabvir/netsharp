using CommandLine;

namespace HW1.Models;

internal class Options
{
    [Option('m', "mode", Required = true, HelpText = "Specify mode of operation: server or client")]
    public OperationMode? Mode { get; set; }

    [Option('u', "username", HelpText = "Specify username (required for client mode)")]
    public string? Username { get; set; }

    [Option('h', "help", HelpText = "Display this help")]
    public bool ShowHelp { get; set; }
}


enum OperationMode
{
    Server,
    Client
}