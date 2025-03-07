using Microsoft.AspNetCore.Http;
using ResultType.Errors;

namespace ResultType;

/// <summary>
/// Provides extension methods for handling API result transformations.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Converts a <see cref="Result{TValue}"/> into an <see cref="IResult"/> suitable for ASP.NET Core minimal APIs.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <returns>
    /// An <see cref="IResult"/> representing a successful HTTP response if the operation was successful,
    /// or an error response based on the associated <see cref="IError"/>.
    /// </returns>
    public static IResult ToHttpResult<TValue>(this Result<TValue> result)
    {
        return result.Match(
            Results.Ok,
            ExtractErrorResult
        );
    }
    
    /// <summary>
    /// Matches a non-generic <see cref="Result"/> and executes the appropriate function based on success or failure.
    /// </summary>
    /// <typeparam name="TOut">The type of the output value.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <param name="onSuccess">Function to execute when the result is successful.</param>
    /// <param name="onFailure">Function to execute when the result contains an error.</param>
    /// <returns>The output of either <paramref name="onSuccess"/> or <paramref name="onFailure"/>.</returns>
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<IError, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error!);
    }

    /// <summary>
    /// Matches a generic <see cref="Result{TValue}"/> and executes the appropriate function based on success or failure.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    /// <typeparam name="TOut">The type of the output value.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <param name="onSuccess">Function to execute when the result is successful.</param>
    /// <param name="onFailure">Function to execute when the result contains an error.</param>
    /// <returns>The output of either <paramref name="onSuccess"/> or <paramref name="onFailure"/>.</returns>
    private static TOut Match<TValue, TOut>(
        this Result<TValue> result,
        Func<TValue, TOut> onSuccess,
        Func<IError, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error!);
    }
    
    private static readonly Dictionary<int, Func<IError, IResult>> ErrorHandlers = new()
    {
        { StatusCodes.Status400BadRequest, Results.BadRequest },
        { StatusCodes.Status401Unauthorized, _ => Results.Unauthorized() },
        { StatusCodes.Status403Forbidden, _ => Results.Forbid() },
        { StatusCodes.Status404NotFound, Results.NotFound },
        { StatusCodes.Status409Conflict, Results.Conflict },
        { StatusCodes.Status422UnprocessableEntity, Results.UnprocessableEntity }
    };

    /// <summary>
    /// Converts an <see cref="IError"/> into an appropriate HTTP <see cref="IResult"/>.
    /// </summary>
    /// <param name="error">The error to transform.</param>
    /// <returns>
    /// An HTTP response corresponding to the error type, or a generic 500 Internal Server Error response if no matching handler exists.
    /// </returns>
    private static IResult ExtractErrorResult(IError error)
    {
        return error switch
        {
            Error { StatusCode: not null } e when ErrorHandlers.TryGetValue(e.StatusCode.Value, out var handler) =>
                handler(e),
            Error e => Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError),
            ValidationError v => Results.UnprocessableEntity(v),
            _ => Results.Problem("An unexpected error occurred", statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}
