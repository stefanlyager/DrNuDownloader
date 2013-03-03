using Autofac;
using Rtmp;

namespace DrNuDownloader
{
    public class Bootstrapper
    {
        public IContainer Container { get; private set; }

        public void Initialize()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyTypes(typeof(DrNuClient).Assembly)
                            .AsImplementedInterfaces();
            containerBuilder.RegisterAssemblyTypes(typeof(RtmpStream).Assembly)
                            .AsImplementedInterfaces();
            Container = containerBuilder.Build();
        }
    }
}