namespace UrlService.Domain
{
  public class Error
  {
    public string Message { get; }

    public static implicit operator Error(string message) =>
      new Error(message);

    public Error(string message)
    {
      this.Message = message;
    }

    public override string ToString() =>
      Message;
  }
}
