using System;
using System.CommandLine;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

var pluginOption = new Option<string?>("--plugin");

var pluginVersionOption = new Option<string?>("--plugin-version");

var pluginWhateverOption = new Option<string[]>("--plugin-whatever")
{
    AllowMultipleArgumentsPerToken = true,
};

var interactiveCommand = new Command("interactive", "Enter interactive mode (REPL). Exit with 'exit' or 'quit'.");
interactiveCommand.Aliases.Add("i");
interactiveCommand.Aliases.Add("repl");

var rootCommand = new RootCommand("")
{
    pluginOption,
    pluginVersionOption,
    pluginWhateverOption,
    interactiveCommand,
};

rootCommand.SetAction((ParseResult parseResult, CancellationToken cancellationToken) =>
{
    var plugin = parseResult.GetValue(pluginOption);
    var pluginVersion = parseResult.GetValue(pluginVersionOption);
    var pluginWhatever = parseResult.GetValue(pluginWhateverOption);

    Console.WriteLine($"Plugin: {plugin} (null: {plugin is null})");
    Console.WriteLine($"Plugin Version: {pluginVersion} (null: {pluginVersion is null})");
    Console.WriteLine($"Plugin Whatever: {pluginWhatever?.Length} (null: {pluginWhatever is null})");
    Console.WriteLine($"Is Cancellation Requested: {cancellationToken.IsCancellationRequested}");

    return Task.CompletedTask;
});

interactiveCommand.SetAction(
    async (_, cancellationToken) =>
    {
        while (true)
        {
            Console.Write("> ");
            var inputLine = Console.ReadLine()?.Trim();

            // Verify input
            if (inputLine is null or "exit" or "quit")
                break;
            if (inputLine is "i" or "interactive" or "repl")
            {
                Console.WriteLine("🛑 I see what you're trying to do!");
                continue;
            }

            await rootCommand.Parse(inputLine).InvokeAsync(cancellationToken);
        }
    });

Console.OutputEncoding = Encoding.UTF8;
await rootCommand.Parse(args).InvokeAsync();
