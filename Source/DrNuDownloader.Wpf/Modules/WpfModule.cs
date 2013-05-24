using Autofac;

namespace DrNuDownloader.Wpf.Modules
{
    public class WpfModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(typeof(WpfModule).Assembly)
                            .AsImplementedInterfaces();
        }
    }
}