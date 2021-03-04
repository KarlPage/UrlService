using System;

namespace UrlService.Domain
{
  /// <summary>
  /// Allow error information to be associated with a type.
  /// </summary>
  public class Result<T>
  {
    private Error Error { get; }
    private T Value { get; }

    public bool IsFail => !IsOk;
    public bool IsOk => null == Error;    

    public static implicit operator Result<T>(T value) =>
      new Result<T>(value, null);

    public static Result<T> Fail(Error error) =>
      new Result<T>(default, error);

    public static Result<T> Ok(T value) =>
      new Result<T>(value, null);

    public Result<R> Then<R>(Func<T, R> map) =>
      IsOk ? Result<R>.Ok(map(Value)) : Result<R>.Fail(Error);

    public Result<R> Then<R>(Func<T, Result<R>> bind) =>
      IsOk ? bind(Value) : Result<R>.Fail(Error);    

    public R Match<R>(Func<Error, R> onFail, Func<T, R> onOk) =>
      IsOk ? onOk(Value) : onFail(Error);

    public void Visit(Action<Error> onFail, Action<T> onOk)
    {
      if (IsOk)
        onOk(Value);
      else
        onFail(Error);
    }

    public override string ToString() =>
      IsOk ? $"{Value}" : $"{Error}";

    private Result(T value, Error error)
    {
      this.Value = value;
      this.Error = error;
    }
  }

  public static class Result
  {
    public static Result<T> ToResult<T>(this T value) =>
      Result<T>.Ok(value);

    public static Result<string> ToResult(this string value, Func<Error> ifNullOrEmpty) =>
      string.IsNullOrEmpty(value) ? Result<string>.Fail(ifNullOrEmpty()) : value;
  }
}
