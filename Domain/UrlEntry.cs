namespace UrlService.Domain
{
  /// <summary>
  /// Maps URL key to long URL
  /// </summary>
  public class UrlEntry
  {
    public UrlKey Key { get; }
    public string Url { get; }

    public static Result<UrlEntry> CreateFromUrl(int keyLength, string url)
    {
      return url
        .ToResult(() => "URL cannot be null or empty")
        .Then(v => UrlKey.Create(keyLength))
        .Then(key => new UrlEntry(key, url));
    }

    public UrlEntry(UrlKey key, string url)
    {
      this.Key = key;
      this.Url = url;
    }    

    public override string ToString() =>
      $"{Key} = ${Url}";
  }
}
