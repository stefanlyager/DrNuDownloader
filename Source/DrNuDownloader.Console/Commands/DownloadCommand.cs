using System;
using System.Globalization;

namespace DrNuDownloader.Console.Commands
{
    public interface IDownloadCommand : ICommand { }

    public class DownloadCommand : IDownloadCommand
    {
        public delegate IDownloadCommand Factory(Uri downloadUri);

        private readonly IDrNuClient _drNuClient;
        private readonly Uri _downloadUri;

        public DownloadCommand(IDrNuClient drNuClient, Uri downloadUri)
        {
            if (drNuClient == null) throw new ArgumentNullException("drNuClient");
            if (downloadUri == null) throw new ArgumentNullException("downloadUri");

            _drNuClient = drNuClient;
            _downloadUri = downloadUri;
        }

        public void Execute()
        {
            TimeSpan duration = TimeSpan.MinValue;
            _drNuClient.Duration += (sender, eventArgs) => { duration = eventArgs.Duration; };
            _drNuClient.Elapsed += (sender, eventArgs) =>
            {
                System.Console.CursorLeft = 0;

                double progressInPercent = 0;
                if (duration != TimeSpan.MinValue)
                {
                    progressInPercent = (double)eventArgs.Elapsed.Ticks / duration.Ticks * 100;
                }

                System.Console.Write("Downloading RTMP stream at {0} of {1} ({2}%).",
                                     eventArgs.Elapsed.ToString(@"hh\:mm\:ss"),
                                     duration.ToString(@"hh\:mm\:ss"),
                                     progressInPercent.ToString("##0.00", CultureInfo.InvariantCulture));
            };

            _drNuClient.Download(_downloadUri);
        }
    }
}