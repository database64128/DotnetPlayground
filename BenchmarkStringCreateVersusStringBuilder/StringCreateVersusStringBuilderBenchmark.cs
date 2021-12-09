using BenchmarkDotNet.Attributes;
using System.Text;

namespace BenchmarkStringCreateVersusStringBuilder;

[MemoryDiagnoser]
public class StringCreateVersusStringBuilderBenchmark
{
    [Params(100, 1000, 10000)]
    public int Lines { get; set; }

    public const string Message = "[WARN] Logging efficiency matters!";

    [Benchmark]
    public void StringCreate()
    {
        _ = string.Create(Lines * (Message.Length + Environment.NewLine.Length), Message, (buf, msg) =>
        {
            for (var i = 0; i < Lines; i++)
            {
                msg.CopyTo(buf[(i * (Message.Length + Environment.NewLine.Length))..]);
                Environment.NewLine.CopyTo(buf[(i * (Message.Length + Environment.NewLine.Length) + Message.Length)..]);
            }
        });
    }

    [Benchmark(Baseline = true)]
    public void StringBuilder()
    {
        var sb = new StringBuilder(Lines * (Message.Length + Environment.NewLine.Length));
        for (var i = 0; i < Lines; i++)
        {
            sb.AppendLine(Message);
        }
        _ = sb.ToString();
    }
}
