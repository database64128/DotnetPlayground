using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Buffers.Text;
using System.Text;

namespace BenchmarkParsingStringIntoOtherTypes;

[MemoryDiagnoser]
public class StringParsingPreSetupBenchmark
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

    public byte[] Utf8String { get; set; } = Array.Empty<byte>();

    [GlobalSetup(Target = nameof(Utf8ParserTryParse))]
    public void GlobalSetupByteArray()
    {
        var length = Encoding.UTF8.GetByteCount(IntString);
        Utf8String = ArrayPool<byte>.Shared.Rent(length);
        _ = Encoding.UTF8.GetBytes(IntString, Utf8String);
    }

    [GlobalCleanup(Target = nameof(Utf8ParserTryParse))]
    public void GlobalCleanupByteArray() => ArrayPool<byte>.Shared.Return(Utf8String);

    [Benchmark]
    public void Utf8ParserTryParse()
    {
        if (!Utf8Parser.TryParse(Utf8String, out int _, out _))
        {
            Console.WriteLine($"Error: failed to parse {IntString}");
        }
    }
}
