using System;
using System.CommandLine;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrimUsingFileStream
{
    internal class Program
    {
        private static Task<int> Main(string[] args)
        {
            var s = "Trimming using FileStream causes the error \"Stream does not support writing.\" or \"Cannot access a closed file.\" thrown at runtime.";
            var testObj = new TestType()
            {
                Id = Guid.NewGuid().ToString(),
                Name = s,
            };
            var rootCommand = new CliRootCommand(s);
            rootCommand.SetAction(async (_, cancellationToken) =>
            {
                await using var afs = new FileStream("test-trim-await-using-fs.json", FileMode.Create);
                await JsonSerializer.SerializeAsync(afs, testObj, cancellationToken: cancellationToken);

                using var sfs = new FileStream("test-trim-using-fs.json", FileMode.Create);
                await JsonSerializer.SerializeAsync(sfs, testObj, cancellationToken: cancellationToken);
            });

            Console.OutputEncoding = Encoding.UTF8;
            return rootCommand.Parse(args).InvokeAsync();
        }

        public static readonly JsonSerializerOptions camelCaseJsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
    }

    public class TestType
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
