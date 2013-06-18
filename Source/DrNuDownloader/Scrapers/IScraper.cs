namespace DrNuDownloader.Scrapers
{
    public interface IScraper<in TInput, out TOutput>
    {
        TOutput Scrape(TInput input);
    }
}