using System;
using System.Globalization;
using System.Text;
using DrNuDownloader.Scrapers;

namespace DrNuDownloader.Console.Commands
{
    public interface IDownloadCommand : ICommand { }

    public class DownloadCommand : BaseCommand, IDownloadCommand
    {
        public delegate IDownloadCommand Factory(Uri downloadUri);

        private readonly IDrNuClient _drNuClient;
        private readonly IFileNameSanitizer _fileNameSanitizer;
        private readonly Uri _downloadUri;

        public DownloadCommand(IDrNuClient drNuClient, IFileNameSanitizer fileNameSanitizer, Uri downloadUri)
        {
            if (drNuClient == null) throw new ArgumentNullException("drNuClient");
            if (fileNameSanitizer == null) throw new ArgumentNullException("fileNameSanitizer");
            if (downloadUri == null) throw new ArgumentNullException("downloadUri");

            _drNuClient = drNuClient;
            _fileNameSanitizer = fileNameSanitizer;
            _downloadUri = downloadUri;
        }

        public override void Execute()
        {
            base.Execute();

            try
            {
                var program = _drNuClient.GetProgram(_downloadUri);
                if (!string.IsNullOrWhiteSpace(program.Title))
                {
                    System.Console.WriteLine("Downloading \"{0}\" from DR NU.", program.Title);
                    System.Console.WriteLine();
                }

                TimeSpan duration = TimeSpan.MinValue;
                program.Duration += (sender, eventArgs) =>
                {
                    duration = eventArgs.Duration;
                    WriteProgressBar(duration, TimeSpan.MinValue, 0);
                };

                long bytesDownloaded = 0;
                program.Elapsed += (sender, eventArgs) =>
                {
                    bytesDownloaded = eventArgs.Bytes;

                    System.Console.CursorLeft = 0;
                    System.Console.CursorTop -= 2;
                    WriteProgressBar(duration, eventArgs.Elapsed, eventArgs.Bytes);
                };

                string sanitizedTitle = _fileNameSanitizer.Sanitize(program.Title);
                if (string.IsNullOrWhiteSpace(sanitizedTitle))
                {
                    sanitizedTitle = "Program";
                }

                program.Download(string.Format("{0}.flv", sanitizedTitle));

                System.Console.CursorLeft = 0;
                System.Console.CursorTop -= 2;
                WriteProgressBar(duration, duration, bytesDownloaded);
            }
            catch (ScraperException)
            {
                System.Console.WriteLine("Der blev ikke fundet noget TV-program på den pågældende URL.");
            }
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