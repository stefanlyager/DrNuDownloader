using System;
using Args;
using Args.Help;
using Autofac;

namespace DrNuDownloader.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Initialize();

            var drNuClient = bootstrapper.Container.Resolve<IDrNuClient>();

            var modelBindingDefinition = Configuration.Configure<Arguments>();
            try
            {
                var arguments = modelBindingDefinition.CreateAndBind(args);
                if (arguments.ListUri != null)
                {
                    var episodeUris = drNuClient.GetEpisodeUris(arguments.ListUri);
                    foreach (var uri in episodeUris)
                    {
                        System.Console.WriteLine(uri);
                    }
                }
                else if (arguments.DownloadUri != null)
                {
                    drNuClient.Download(arguments.DownloadUri);
                }
                else if (arguments.DownloadAllUri != null)
                {
                    var episodeUris = drNuClient.GetEpisodeUris(arguments.DownloadAllUri);
                    foreach (var uri in episodeUris)
                    {
                        drNuClient.Download(uri);
                    }
                }
                else
                {
                    OutputHelpText(modelBindingDefinition);
                }
            }
            catch (InvalidArgsFormatException)
            {
                OutputHelpText(modelBindingDefinition);
            }
        }

        private static void OutputHelpText(IModelBindingDefinition<Arguments> modelBindingDefinition)
        {
            if (modelBindingDefinition == null) throw new ArgumentNullException("modelBindingDefinition");

            var helpProvider = new HelpProvider();
            var modelHelp = helpProvider.GenerateModelHelp(modelBindingDefinition);

            var helpFormatter = new HelpFormatter();
            helpFormatter.WriteHelp(modelHelp, System.Console.Out);
        }
    }
}