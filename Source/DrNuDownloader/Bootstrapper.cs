using Autofac;

namespace DrNuDownloader
{
    public class Bootstrapper
    {
        public IContainer Container { get; private set; }

        public void Initialize()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<DrNuClient>().As<IDrNuClient>();
            containerBuilder.RegisterType<ProgramClient>().As<IProgramClient>();
            containerBuilder.RegisterType<EpisodeListClient>().As<IEpisodeListClient>();
            containerBuilder.RegisterType<EpisodeClient>().As<IEpisodeClient>();
            containerBuilder.RegisterType<ResourceClient>().As<IResourceClient>();
            Container = containerBuilder.Build();
        }
    }
}