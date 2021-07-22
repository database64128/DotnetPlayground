using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;

var pluginOption = new Option<string>("--plugin");
var pluginVersionOption = new Option<string>("--plugin-version");
var pluginWhateverOption = new Option<string[]>("--plugin-whatever");

var rootCommand = new RootCommand("")
{
    pluginOption,
    pluginVersionOption,
    pluginWhateverOption,
};

rootCommand.Handler = CommandHandler.Create<string, string, string, string[]>(
    (string plugin, string pluginVersion, string pluginOptions, string[] pluginWhatever) =>
    {
        Console.WriteLine($"Plugin: {plugin}");
        Console.WriteLine($"Plugin Version: {pluginVersion}");
        Console.WriteLine($"Plugin Whatever: {pluginWhatever.Length}");
    });

Console.OutputEncoding = Encoding.UTF8;
await rootCommand.InvokeAsync(args);
