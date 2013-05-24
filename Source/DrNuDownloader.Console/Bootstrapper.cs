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
            containerBuilder.RegisterAssemblyModules(typeof(RtmpStream).Assembly);
            containerBuilder.RegisterAssemblyModules(typeof(DrNuClient).Assembly);
            containerBuilder.RegisterAssemblyModules(typeof(Bootstrapper).Assembly);
            Container = containerBuilder.Build();
        }
    }
}