using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

Console.WriteLine("Simple streaming test");

var myFirst = StreamAsync();
var mySecond = StreamAsync();
var myThird = AsyncEnumerableEx.Merge(myFirst, mySecond);

await foreach (var result in myThird)
{
    Console.WriteLine(result);
}

Console.WriteLine("Simple task test");

Console.WriteLine("Do it all");

var tasks = new List<Task<string>>()
{
    SimpleTaskAsync(),
    SimpleTaskAsync(),
    SimpleTaskAsync(),
    SimpleTaskAsync(),
    SimpleTaskAsync(),
};

var doItAll = await Task.WhenAll(tasks);
foreach (var result in doItAll)
{
    Console.WriteLine(result);
}

Console.WriteLine("Stream the tasks");

var simpleArray = new string[] { "a", "b", "c", "d", "e" };

var results = ConcurrentMerge(simpleArray.Select(_ => StreamAsync()));
var resultsNested = ConcurrentMergeNested(simpleArray.ToAsyncEnumerable().Select(_ => StreamAsync()));
var resultsConcated = AsyncEnumerableEx.Concat(results, resultsNested).Where(x => x is "-1s");

await foreach (var result in resultsConcated)
{
    Console.WriteLine(result);
}

static async IAsyncEnumerable<string> StreamAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
{
    await Task.Delay(1000, cancellationToken);
    yield return "-1s";
}

static async Task<string> SimpleTaskAsync(CancellationToken cancellationToken = default)
{
    await Task.Delay(1000, cancellationToken);
    return "-1s";
}

/// <summary>
/// Merges elements from all async-enumerable sequences in the given enumerable sequence into a single async-enumerable sequence.
/// </summary>
/// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
/// <param name="sources">Enumerable sequence of async-enumerable sequences.</param>
/// <returns>The async-enumerable sequence that merges the elements of the async-enumerable sequences.</returns>
/// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
static IAsyncEnumerable<TSource> ConcurrentMerge<TSource>(IEnumerable<IAsyncEnumerable<TSource>> sources)
{
    ArgumentNullException.ThrowIfNull(sources);

    return AsyncEnumerableEx.Merge(sources.ToArray());
}

/// <summary>
/// Merges elements from all inner async-enumerable sequences into a single async-enumerable sequence.
/// </summary>
/// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
/// <param name="sources">Observable sequence of inner async-enumerable sequences.</param>
/// <returns>The async-enumerable sequence that merges the elements of the inner sequences.</returns>
/// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
static IAsyncEnumerable<TSource> ConcurrentMergeNested<TSource>(IAsyncEnumerable<IAsyncEnumerable<TSource>> sources)
{
    ArgumentNullException.ThrowIfNull(sources);

    return AsyncEnumerableEx.Merge(sources.ToEnumerable().ToArray());
}
