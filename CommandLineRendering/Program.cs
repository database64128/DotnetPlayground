using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Threading.Tasks;

namespace CommandLineRendering
{
    internal class Program
    {
        private static Task Main(string[] args)
        {
            var rootCommand = new RootCommand("Rendering test.")
            {
                Handler = CommandHandler.Create<InvocationContext>(Render)
            };

            return rootCommand.InvokeAsync(args);
        }

        private static void Render(InvocationContext invocationContext)
        {
            var console = invocationContext.Console;
            var consoleRenderer = new ConsoleRenderer(console);

            var table = new TableView<User>()
            {
                Items = new User[]
                {
                    new(0, "ian", "dev"),
                    new(1, "Gracie", "gra"),
                    new(2, "Tinashe", "tin"),
                },
            };

            table.AddColumn(x => x.Id, "ID");
            table.AddColumn(x => x.Name, "Name");
            table.AddColumn(x => x.Description, "Description");

            using var screen = new ScreenView(consoleRenderer, console)
            {
                Child = table,
            };

            screen.Render();
        }
    }

    public record User(int Id, string Name, string Description);
}
