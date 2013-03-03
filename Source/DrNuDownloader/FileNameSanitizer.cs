using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DrNuDownloader
{
    public interface IFileNameSanitizer
    {
        string Sanitize(string fileName);
    }

    public class FileNameSanitizer : IFileNameSanitizer
    {
        public string Sanitize(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");

            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string pattern = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return Regex.Replace(fileName, pattern, "_");
        }
    }
}