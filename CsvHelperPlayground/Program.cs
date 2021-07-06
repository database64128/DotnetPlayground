using CsvHelper;
using System.Globalization;
using System.IO;

var arr = new (int id, string name)[]
{
    (0, "damn"),
    (1, "what"),
    (2, "ok"),
};

await using var writer = new StreamWriter("test.csv");
await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
await csv.WriteRecordsAsync(arr);
