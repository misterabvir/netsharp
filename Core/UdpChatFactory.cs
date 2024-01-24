using CommandLine;
using App.Infrastructure;

namespace App.Core;

internal class UdpChatFactory
{
    private readonly string[] _args;
    private readonly ILogger _logger;
    private readonly IInput _input;
    private UdpChat? _chat;

    private UdpChatFactory(string[] args, ILogger logger, IInput input)
    {
        _args = args;
        _logger = logger;
        _input = input;
    }

    public static UdpChat Create(string[] args, ILogger logger, IInput input)
    {
        UdpChatFactory factory = new(args, logger, input);
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
            OperationMode.Client => new UdpChatClient(options.Username!, _logger, _input),
            OperationMode.Server => new UdpChatServer(_logger, _input),
            _ => null
        };
    }
}

