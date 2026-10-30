using CsvHelper;

var arr = new (int id, string name)[]
{
    (0, "damn"),
    (1, "what"),
    (2, "ok"),
};

await using var csv = CsvWriter.ToFile("test.csv");
await csv.WriteRecordsAsync(arr);
