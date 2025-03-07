namespace ResultType.Errors;

public class Error : IError
{
    public Error(string code, string message, int? statusCode)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        StatusCode = statusCode;
    }

    public string Code { get; set; }
    public string Message { get; set; }
    public int? StatusCode { get; }
}