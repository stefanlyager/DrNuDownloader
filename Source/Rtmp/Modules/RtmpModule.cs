using Autofac;

namespace Rtmp.Modules
{
    public class RtmpModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(GetType().Assembly)
                            .AsImplementedInterfaces();
        }
    }
}