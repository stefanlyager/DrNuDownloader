using System;

namespace DrNuDownloader.Console.Commands
{
    public interface IListCommand : ICommand { }

    public class ListCommand : BaseCommand, IListCommand
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

        public override void Execute()
        {
            base.Execute();

            System.Console.WriteLine("Videos found at {0}:", _listUri);
            System.Console.WriteLine();

            var episodeUris = _drNuClient.GetProgramUris(_listUri);
            foreach (var uri in episodeUris)
            {
                System.Console.WriteLine(uri);
            }
        }
    }
}