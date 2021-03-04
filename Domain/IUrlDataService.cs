using System.Threading.Tasks;

namespace UrlService.Domain
{
  public interface IUrlDataService
  {
    /// <summary>
    /// Obtain full URL given the short URL key.
    /// </summary>    
    Result<UrlEntry> GetUrl(UrlKey key);

    /// <summary>
    /// Add URL to database using the entry URL key and full URL.
    /// </summary>
    Result<UrlEntry> AddShortUrl(UrlEntry entry);
  }
}
