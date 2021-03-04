using System;

namespace UrlService.Domain
{
  /// <summary>
  /// UrlKey forms part of the small URL.
  /// Used by database to lookup long URL.
  /// </summary>
  public class UrlKey
  {
    public string Value { get; }

    public static UrlKey Create(int keyLength)
    {
      string key = Guid.NewGuid().ToString("N").Substring(0, keyLength).ToLower();
      return new UrlKey(key);
    }
    
    public static Result<UrlKey> FromString(string key)
    {
      return key
        .ToResult(() => "UrlKey cannot be null or empty.")
        .Then(v => new UrlKey(v));
    }

    public override string ToString() =>
      Value;

    private UrlKey(string value)
    {
      this.Value = value;
    }
  }
}
