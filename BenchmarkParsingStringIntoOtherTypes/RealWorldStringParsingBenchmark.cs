using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Buffers.Text;
using System.Text;

namespace BenchmarkParsingStringIntoOtherTypes;

[MemoryDiagnoser]
public class RealWorldStringParsingBenchmark
{
    [Params("Too Many Requests: retry after 11")]
    public string Message { get; set; } = "";

    [Benchmark(Baseline = true)]
    public void GetIntFromStringOld()
    {
        // "Too Many Requests: retry after 11"
        if (Message.Length > 31)
        {
            var timeString = Message[31..];
            if (int.TryParse(timeString, out _))
            {
                return;
            }
        }

        Console.WriteLine($"Error: failed to parse {Message}");
    }

    [Benchmark]
    public void GetIntFromStringNew()
    {
        // "Too Many Requests: retry after 11"
        var length = Encoding.UTF8.GetByteCount(Message);
        if (length > 31)
        {
            var array = ArrayPool<byte>.Shared.Rent(length);
            Span<byte> timeString = array;
            try
            {
                _ = Encoding.UTF8.GetBytes(Message, timeString);
                timeString = timeString[31..];
                if (Utf8Parser.TryParse(timeString, out int timeSec, out _))
                {
                    return;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }

        Console.WriteLine($"Error: failed to parse {Message}");
    }

    [Benchmark]
    public void GetIntFromStringMixed()
    {
        // "Too Many Requests: retry after 11"
        ReadOnlySpan<char> msg = Message;
        if (msg.Length > 31)
        {
            var timeString = msg[31..];
            if (int.TryParse(timeString, out _))
            {
                return;
            }
        }

        Console.WriteLine($"Error: failed to parse {Message}");
    }
}
