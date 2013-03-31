using Autofac;
using DrNuDownloader.Console.Commands;

namespace DrNuDownloader.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Initialize();

            var commandFactory = bootstrapper.Container.Resolve<ICommandFactory>();
            ICommand command = commandFactory.CreateCommand(args);
            command.Execute();
        }
    }
}