using Autofac;

namespace DrNuDownloader.Console.Modules
{
    public class ConsoleModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(typeof(ConsoleModule).Assembly)
                            .AsImplementedInterfaces();
        }
    }
}