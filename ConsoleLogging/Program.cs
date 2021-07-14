using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace ConsoleLogging
{
    class Program
    {
        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("ConsoleLogging", LogLevel.Debug)
                    .AddSimpleConsole(options =>
                    {
                        options.SingleLine = true;
                    });
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();
            Console.OutputEncoding = Encoding.UTF8;
            logger.LogInformation("Hello world! 🌐 测试");
            Console.WriteLine("Hello world! 🌐 测试");
        }
    }
}
