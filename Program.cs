using CommandLine;
using CommandLine.Text;
using HW1.Chat;
using HW1.Models;
using System.Net;

namespace HW1;

public class Program
{
    private static UdpChat? _chat;


    static async Task Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseError);

        if (_chat is not null)
            await _chat.RunAsync();
    }

    static void HandleParseError(IEnumerable<Error> errors)
    {
        foreach (var error in errors)
        {
            Console.WriteLine(error);
        }
    }

    static void RunOptions(Options options)
    {
        if (options.ShowHelp)
        {
            return;
        }


        if (options.Mode == OperationMode.Client && string.IsNullOrEmpty(options.Username))
        {
            Console.WriteLine("Username is required for client mode.");
            return;
        }


        switch (options.Mode)
        {
            case OperationMode.Server:
                _chat = new Server();
                break;
            case OperationMode.Client:
                _chat = new Client(options.Username!);
                break;
            default:
                break;
        }
    }
}
