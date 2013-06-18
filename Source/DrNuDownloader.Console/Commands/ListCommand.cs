using System;

namespace DrNuDownloader.Console.Commands
{
    public interface IListCommand : ICommand { }

    public class ListCommand : IListCommand
    {
        public delegate IListCommand Factory(Uri listUri);

        private readonly IDrNuClient _drNuClient;
        private readonly Uri _listUri;

        public ListCommand(IDrNuClient drNuClient, Uri listUri)
        {
            if (drNuClient == null) throw new ArgumentNullException("drNuClient");
            if (listUri == null) throw new ArgumentNullException("listUri");

            _drNuClient = drNuClient;
            _listUri = listUri;
        }

        public void Execute()
        {
            var episodeUris = _drNuClient.GetProgramUris(_listUri);
            foreach (var uri in episodeUris)
            {
                System.Console.WriteLine(uri);
            }
        }
    }
}