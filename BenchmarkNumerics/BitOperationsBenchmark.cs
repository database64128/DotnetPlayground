#pragma warning disable CA1822 // Mark members as static

using BenchmarkDotNet.Attributes;
using System.Numerics;

namespace BenchmarkNumerics;

[MemoryDiagnoser]
public class BitOperationsBenchmark
{
    public const ulong TestNumber = 1UL << 63;

    [Benchmark(Baseline = true)]
    public void IsPow2() => BitOperations.IsPow2(TestNumber);

    [Benchmark]
    public void IsPow2Popcnt() => IsPow2PopcntImpl(TestNumber);

    private static bool IsPow2PopcntImpl(ulong ul) => BitOperations.PopCount(ul) == 1;
}
