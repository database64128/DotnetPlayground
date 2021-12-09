using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Buffers.Text;
using System.Text;

namespace BenchmarkParsingStringIntoOtherTypes;

[MemoryDiagnoser]
public class StringParsingBenchmark
{
    [Params("1", "64", "10000000")]
    public string IntString { get; set; } = "";

    [Benchmark(Baseline = true)]
    public void IntTryParse()
    {
        if (!int.TryParse(IntString, out _))
        {
            Console.WriteLine($"Error: failed to parse {IntString}");
        }
    }

    [Benchmark]
    public void Utf8ParserTryParse()
    {
        var length = Encoding.UTF8.GetByteCount(IntString);
        var array = ArrayPool<byte>.Shared.Rent(length);
        Span<byte> intUtf8String = array;
        try
        {
            _ = Encoding.UTF8.GetBytes(IntString, intUtf8String);
            if (!Utf8Parser.TryParse(intUtf8String, out int _, out _))
            {
                Console.WriteLine($"Error: failed to parse {IntString}");
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }
}
