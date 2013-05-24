using System;
using System.Globalization;
using System.Text;

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
            System.Console.CursorVisible = false;
            System.Console.WriteLine("DR NU Downloader");
            System.Console.WriteLine("Copyright (c) 2012 Stefan Lyager");
            System.Console.WriteLine();

            TimeSpan duration = TimeSpan.MinValue;
            _drNuClient.Duration += (sender, eventArgs) =>
            {
                duration = eventArgs.Duration;
                WriteProgressBar(duration, TimeSpan.MinValue, 0);
            };

            long bytesDownloaded = 0;
            _drNuClient.Elapsed += (sender, eventArgs) =>
            {
                bytesDownloaded = eventArgs.Bytes;

                System.Console.CursorLeft = 0;
                System.Console.CursorTop -= 2;
                WriteProgressBar(duration, eventArgs.Elapsed, eventArgs.Bytes);
            };

            _drNuClient.Download(_downloadUri);

            System.Console.CursorLeft = 0;
            System.Console.CursorTop -= 2;
            WriteProgressBar(duration, duration, bytesDownloaded);
        }

        private void WriteProgressBar(TimeSpan duration, TimeSpan elapsed, double bytesDownloaded)
        {
            var progressBarStringBuilder = new StringBuilder();

            double progressInPercent = 0;
            if (duration != TimeSpan.MinValue && elapsed != TimeSpan.MinValue)
            {
                progressInPercent = (double)elapsed.Ticks / duration.Ticks * 100;
            }

            progressBarStringBuilder.Append("[");

            var progressedTicks = (int)Math.Floor(progressInPercent / 2);
            for (int i = 1; i <= 50; i++)
            {
                if (i <= progressedTicks)
                {
                    progressBarStringBuilder.Append(i < progressedTicks ? "=" : ">");
                }
                else
                {
                    progressBarStringBuilder.Append(".");
                }
            }

            progressBarStringBuilder.Append("]");

            progressBarStringBuilder.AppendLine(string.Format(" ({0}%)", progressInPercent.ToString("##0.00", CultureInfo.InvariantCulture)));
            progressBarStringBuilder.AppendLine();
            progressBarStringBuilder.Append(string.Format("{0} kB downloaded ({1} out of {2} total)",
                                                          Math.Floor(bytesDownloaded / 1024),
                                                          elapsed.ToString(@"hh\:mm\:ss"),
                                                          duration.ToString(@"hh\:mm\:ss")));
            System.Console.Write(progressBarStringBuilder.ToString());
        }
    }
}