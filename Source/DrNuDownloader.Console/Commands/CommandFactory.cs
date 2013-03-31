using System;
using Args;

namespace DrNuDownloader.Console.Commands
{
    public interface ICommandFactory
    {
        ICommand CreateCommand(string[] args);
    }

    public class CommandFactory : ICommandFactory
    {
        private readonly ListCommand.Factory _listCommandFactory;
        private readonly DownloadCommand.Factory _downloadCommandFactory;
        private readonly DownloadAllCommand.Factory _downloadAllCommandFactory;

        public CommandFactory(ListCommand.Factory listCommandFactory, DownloadCommand.Factory downloadCommandFactory, DownloadAllCommand.Factory downloadAllCommandFactory)
        {
            if (listCommandFactory == null) throw new ArgumentNullException("listCommandFactory");
            if (downloadCommandFactory == null) throw new ArgumentNullException("downloadCommandFactory");
            if (downloadAllCommandFactory == null) throw new ArgumentNullException("downloadAllCommandFactory");

            _listCommandFactory = listCommandFactory;
            _downloadCommandFactory = downloadCommandFactory;
            _downloadAllCommandFactory = downloadAllCommandFactory;
        }

        public ICommand CreateCommand(string[] args)
        {
            var modelBindingDefinition = Configuration.Configure<Arguments>();

            try
            {
                Arguments arguments = modelBindingDefinition.CreateAndBind(args);
                if (arguments.ListUri != null)
                {
                    return _listCommandFactory(arguments.ListUri);
                }

                if (arguments.DownloadUri != null)
                {
                    return _downloadCommandFactory(arguments.DownloadUri);
                }

                if (arguments.DownloadAllUri != null)
                {
                    return _downloadAllCommandFactory(arguments.DownloadAllUri);
                }
            }
            catch (InvalidArgsFormatException) { }

            return new HelpCommand(modelBindingDefinition);
        }
    }
}