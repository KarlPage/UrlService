using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UrlService.Domain
{
  public class App
  {
    private IUrlDataService DataService { get; }

    public App(IUrlDataService dataService)
    {
      this.DataService = dataService;
    }

    /// <summary>
    /// Add URL to database and return a UrlResult that contains both the original and short URL.
    /// HostString is used to build full short URL.
    /// </summary>
    public Result<UrlResult> AddUrlKey(HostString host, string url)
    {
      var result = UrlEntry
        .CreateFromUrl(8, url)
        .Then(DataService.AddShortUrl)
        .Then(entry => CreateUrlResult(host, entry));
      return result;
    }

    /// <summary>
    /// Obtain URL from database using the supplied key.
    /// HostString is used to build full hort URL.
    /// </summary>    
    public Result<UrlResult> GetUrl(HostString host, string key)
    {
      var result = UrlKey
        .FromString(key)
        .Then(DataService.GetUrl)
        .Then(entry => CreateUrlResult(host, entry));
      return result;
    }

    public static UrlResult CreateUrlResult(HostString host, UrlEntry entry)
    {
      var shortUrl = $"{host.Value}/api/{entry.Key.Value}";
      return new UrlResult { ShortUrl = shortUrl, LongUrl = entry.Url };
    }
  }
}
