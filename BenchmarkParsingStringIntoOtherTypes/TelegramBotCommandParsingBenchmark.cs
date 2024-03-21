using BenchmarkDotNet.Attributes;

namespace BenchmarkParsingStringIntoOtherTypes;

[MemoryDiagnoser]
public class TelegramBotCommandParsingBenchmark
{
    [Params("/start@database64128 benchmark")]
    public string Text { get; set; } = "";

    [Params("database64128")]
    public string Username { get; set; } = "";

    [Benchmark(Baseline = true)]
    public void ParseOld() => _ = ParseMessageIntoCommandAndArgumentOld(Text, Username);

    [Benchmark]
    public void ParseNew() => _ = ParseMessageIntoCommandAndArgumentNew(Text, Username);

    private static (string? command, string? argument) ParseMessageIntoCommandAndArgumentOld(string? text, string botUsername)
    {
        // Empty message
        if (string.IsNullOrWhiteSpace(text))
            return (null, null);

        // Not a command
        if (!text.StartsWith('/') || text.Length < 2)
            return (null, null);

        // Remove the leading '/'
        text = text[1..];

        // Split command and argument
        var parsedText = text.Split(' ', 2);
        string command;
        string? argument = null;
        switch (parsedText.Length)
        {
            case <= 0:
                return (null, null);
            case 2:
                argument = parsedText[1];
                goto default;
            default:
                command = parsedText[0];
                break;
        }

        // Verify and remove trailing '@bot' from command
        var atSignIndex = command.IndexOf('@');
        if (atSignIndex != -1)
        {
            var atUsername = command[atSignIndex..];
            if (atUsername != $"@{botUsername}")
                return (null, null);

            command = command[..atSignIndex];
        }

        // Trim leading and trailing spaces from argument
        if (argument is not null)
        {
            argument = argument.Trim();
        }

        return (command, argument);
    }

    private static (string? command, string? argument) ParseMessageIntoCommandAndArgumentNew(ReadOnlySpan<char> text, string botUsername)
    {
        // Empty message
        if (text.IsEmpty)
            return (null, null);

        // Not a command
        if (text[0] != '/' || text.Length < 2)
            return (null, null);

        // Remove the leading '/'
        text = text[1..];

        // Split command and argument
        ReadOnlySpan<char> command, argument;
        var spacePos = text.IndexOf(' ');
        if (spacePos == -1)
        {
            command = text;
            argument = [];
        }
        else if (spacePos == text.Length - 1)
        {
            command = text[..spacePos];
            argument = [];
        }
        else
        {
            command = text[..spacePos];
            argument = text[(spacePos + 1)..];
        }

        // Verify and remove trailing '@bot' from command
        var atSignIndex = command.IndexOf('@');
        if (atSignIndex != -1)
        {
            if (atSignIndex != command.Length - 1)
            {
                var atUsername = command[(atSignIndex + 1)..];
                if (!atUsername.SequenceEqual(botUsername))
                {
                    return (null, null);
                }
            }

            command = command[..atSignIndex];
        }

        // Trim leading and trailing spaces from argument
        argument = argument.Trim();

        // Convert back to string
        string? commandString = null;
        string? argumentString = null;
        if (!command.IsEmpty)
        {
            commandString = command.ToString();

            if (!argument.IsEmpty)
            {
                argumentString = argument.ToString();
            }
        }

        return (commandString, argumentString);
    }
}
