namespace UrlService.Domain
{
  /// <summary>
  /// Used to send WEB API response (Json, XML, etc)
  /// </summary>
  public class UrlResult
  {
    public string ShortUrl { get; set; }
    public string LongUrl { get; set; }    
  }
}
