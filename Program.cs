using CommandLine;
using HW1.Chat;
using HW1.Models;
using System.Net;

namespace HW1;

public class Program
{

    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseError);
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
        if (options.Mode == OperationMode.Client && string.IsNullOrEmpty(options.Username))
        {
            Console.WriteLine("Username is required for client mode.");
            return;
        }

        IPAddress address = IPAddress.Parse("127.0.0.1");
        int serverPort = 10000;
        int clientPort = 12000;

        Task.Run(async () =>
        {
            switch (options.Mode)
            {
                case OperationMode.Server:
                    UdpChat server = new Server(config: new() { Address = address, Local = serverPort, Remote = clientPort });
                    await server.Start();
                    break;
                case OperationMode.Client:
                    UdpChat client = new Client(
                        username: options.Username!,
                        config: new() { Address = address, Local = clientPort, Remote = serverPort });
                    await client.Start();
                    break;
                default:
                    break;
            }
        }).Wait();
    }
}