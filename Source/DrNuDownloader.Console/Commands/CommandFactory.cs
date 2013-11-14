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

        public CommandFactory(ListCommand.Factory listCommandFactory, DownloadCommand.Factory downloadCommandFactory)
        {
            if (listCommandFactory == null) throw new ArgumentNullException("listCommandFactory");
            if (downloadCommandFactory == null) throw new ArgumentNullException("downloadCommandFactory");

            _listCommandFactory = listCommandFactory;
            _downloadCommandFactory = downloadCommandFactory;
        }

        public ICommand CreateCommand(string[] args)
        {
            var modelBindingDefinition = Configuration.Configure<Arguments>();

            try
            {
                Arguments arguments = modelBindingDefinition.CreateAndBind(args);
                if (arguments.ListUri != null && arguments.ListUri.IsAbsoluteUri)
                {
                    return _listCommandFactory(arguments.ListUri);
                }

                if (arguments.DownloadUri != null && arguments.DownloadUri.IsAbsoluteUri)
                {
                    return _downloadCommandFactory(arguments.DownloadUri);
                }
            }
            catch (InvalidArgsFormatException) { }

            return new HelpCommand(modelBindingDefinition);
        }
    }
}