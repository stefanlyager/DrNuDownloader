using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DrNuDownloader.Wpf.Annotations;
using Microsoft.Win32;

namespace DrNuDownloader.Wpf.ViewModel
{
    public interface IDownloadViewModel : INotifyPropertyChanged
    {
        Uri Uri { get; set; }
        ICommand DownloadCommand { get; }
    }

    public class DownloadViewModel : IDownloadViewModel
    {
        private readonly IDrNuClient _drNuClient;
        private readonly IFileNameSanitizer _fileNameSanitizer;

        public DownloadViewModel(IDrNuClient drNuClient, IFileNameSanitizer fileNameSanitizer)
        {
            if (drNuClient == null) throw new ArgumentNullException("drNuClient");
            if (fileNameSanitizer == null) throw new ArgumentNullException("fileNameSanitizer");

            _drNuClient = drNuClient;
            _fileNameSanitizer = fileNameSanitizer;
        }

        public Uri Uri { get; set; }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _elapsed;
        public TimeSpan Elapsed
        {
            get { return _elapsed; }
            set
            {
                _elapsed = value;
                OnPropertyChanged();
            }
        }

        public long BytesDownloaded { get; set; }

        private ushort _progress;
        public ushort Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        //public bool CanDownload
        //{
        //    get { throw new NotImplementedException(); }
        //}

        public ICommand DownloadCommand
        {
            get { return new RelayCommand(obj => Download()); }
        }

        private Thread _downloadThread;
        private void Download()
        {
            var program = _drNuClient.GetProgram(Uri);

            string sanitizedTitle = _fileNameSanitizer.Sanitize(program.Title);
            if (string.IsNullOrWhiteSpace(sanitizedTitle))
            {
                sanitizedTitle = "Video";
            }

            var saveFileDialog = new SaveFileDialog
                                     {
                                         AddExtension = true,
                                         FileName = sanitizedTitle,
                                         DefaultExt = ".flv",
                                         Filter = "Flash Video (.flv)|*.flv"
                                     };
            var result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                _downloadThread = new Thread(() =>
                {
                    program.Duration += (sender, args) =>
                    {
                        Duration = args.Duration;
                    };

                    program.Elapsed += (sender, args) =>
                    {
                        BytesDownloaded = args.Bytes;
                        Elapsed = args.Elapsed;

                        Progress = Convert.ToUInt16((double)args.Elapsed.Ticks / Duration.Ticks * 100);
                    };

                    program.Download(saveFileDialog.FileName);

                    Progress = 0;
                    MessageBox.Show("Videoen er nu downloadet.", "Download færdig",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                });

                _downloadThread.Start();
            }
        }

        // TODO: Progress
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}