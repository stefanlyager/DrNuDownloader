using Autofac;
using Rtmp;

namespace DrNuDownloader.Console
{
    public class Bootstrapper
    {
        public IContainer Container { get; private set; }

        public void Initialize()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyTypes(typeof(RtmpStream).Assembly)
                            .AsImplementedInterfaces();
            containerBuilder.RegisterAssemblyTypes(typeof(DrNuClient).Assembly)
                            .AsImplementedInterfaces();
            containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
                            .AsImplementedInterfaces();
            Container = containerBuilder.Build();
        }
    }
}