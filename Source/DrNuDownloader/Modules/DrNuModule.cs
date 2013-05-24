using Autofac;

namespace DrNuDownloader.Modules
{
    public class DrNuModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(typeof(DrNuModule).Assembly)
                            .AsImplementedInterfaces();
        }
    }
}