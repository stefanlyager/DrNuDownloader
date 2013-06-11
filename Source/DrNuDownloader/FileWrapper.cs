using System.IO;

namespace DrNuDownloader
{
    public interface IFileWrapper
    {
        FileStream Create(string path);
    }

    public class FileWrapper : IFileWrapper
    {
        public FileStream Create(string path)
        {
            return File.Create(path);
        }
    }
}