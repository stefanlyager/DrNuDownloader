using System;
using System.Net;

namespace DrNuDownloader.Wrappers
{
    public interface IWebRequestWrapper
    {
        HttpWebRequest CreateHttp(Uri uri);
    }

    public class WebRequestWrapper : IWebRequestWrapper
    {
        public HttpWebRequest CreateHttp(Uri uri)
        {
            return WebRequest.CreateHttp(uri);
        }
    }
}