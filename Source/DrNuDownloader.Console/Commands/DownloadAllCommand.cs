using System;

namespace DrNuDownloader.Console.Commands
{
    public interface IDownloadAllCommand : ICommand { }

    public class DownloadAllCommand : IDownloadAllCommand
    {
        public delegate IDownloadAllCommand Factory(Uri downloadAllUri);

        private readonly IDrNuClient _drNuClient;
        private readonly Uri _downloadAllUri;

        public DownloadAllCommand(IDrNuClient drNuClient, Uri downloadAllUri)
        {
            if (drNuClient == null) throw new ArgumentNullException("drNuClient");
            if (downloadAllUri == null) throw new ArgumentNullException("downloadAllUri");

            _drNuClient = drNuClient;
            _downloadAllUri = downloadAllUri;
        }

        public void Execute()
        {
            var episodeUris = _drNuClient.GetEpisodeUris(_downloadAllUri);
            foreach (var uri in episodeUris)
            {
                _drNuClient.Download(uri);
            }
        }
    }
}