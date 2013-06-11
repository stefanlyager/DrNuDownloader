using System;

namespace DrNuDownloader.Scrapers
{
    public interface IScraper<out T>
    {
        T Scrape(Uri uri);
    }
}