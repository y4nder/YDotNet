using ResultType.Errors;

namespace ResultType;

public class Result
{
    protected Result(bool isSuccess, IError? error)
    {
        switch (error)
        {
            case Error _ when isSuccess:
                throw new InvalidOperationException("Cannot have a general error with a success result.");

            case ValidationError _ when isSuccess:
                throw new InvalidOperationException("Validation errors should only be used for failed results.");

            case null when !isSuccess:
                throw new InvalidOperationException("A failure result must have an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public IError? Error { get; }
    public bool IsFailure => !IsSuccess;
    
    public static Result Success()
        => new Result(true, null);
    
    public static Result Failure(IError error)
        => new Result(false, error);
    
    public static Result<TValue> Success<TValue>(TValue value) 
        =>  new Result<TValue>(value, true, null);
    
    public static Result<TValue> Failure<TValue>(IError error) 
        => new Result<TValue>(default, false, error);
    
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, IError? error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    public static implicit operator Result<TValue>(TValue? value)
    {
        return value is not null
            ? Success(value)
            : Failure<TValue>(new Error("NullValue", "The value is null.", 400));
    }
}