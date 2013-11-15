using System;

namespace DrNuDownloader.Console.Commands
{
    public interface ICommand : IDisposable
    {
        void Execute();
    }
}