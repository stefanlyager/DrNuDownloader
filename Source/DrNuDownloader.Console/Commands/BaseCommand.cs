namespace DrNuDownloader.Console.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public virtual void Execute()
        {
            System.Console.CursorVisible = false;

            System.Console.WriteLine("DR NU Downloader");
            System.Console.WriteLine("Copyright (c) 2012 Stefan Lyager");
            System.Console.WriteLine();
        }

        public void Dispose()
        {
            System.Console.CursorVisible = true;
        }
    }
}