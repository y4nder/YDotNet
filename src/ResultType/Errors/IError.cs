using Microsoft.AspNetCore.Http;

namespace ResultType.Errors;

public interface IError : IStatusCodeHttpResult
{
    public string Code { get; set; }
    public string Message { get; set; }
}