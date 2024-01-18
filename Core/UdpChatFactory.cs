using CommandLine;
using App.Infrastructure;

namespace App.Core;

internal class UdpChatFactory
{
    private readonly string[] _args;
    private UdpChat? _chat;

    private UdpChatFactory(string[] args)
    {
        _args = args;
    }

    public static UdpChat Create(string[] args)
    {
        UdpChatFactory factory = new(args);
        return factory.Create();
    }

    private UdpChat Create()
    {
     
        Parser.Default
            .ParseArguments<Options>(_args)
            .WithParsed(RunOptions);
        return _chat!;
    }


    private void RunOptions(Options options)
    {
        if (options.Mode == OperationMode.Client && string.IsNullOrEmpty(options.Username))
        {
            Console.WriteLine("Username is required for client mode.");
            return;
        }

        _chat = options.Mode switch
        {
            OperationMode.Client => new UdpChatClient(options.Username!),
            OperationMode.Server => new UdpChatServer(),
            _ => null
        };
    }
}

