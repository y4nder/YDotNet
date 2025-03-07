using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace ResultType.Errors;

public class ValidationError : IError
{
    public ValidationError(
        string code,
        string message,
        List<ValidationErrorReason<object>> reasons,
        int? statusCode
    )
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Reasons = reasons ?? throw new ArgumentNullException(nameof(reasons));
        StatusCode = statusCode;
    }

    public List<ValidationErrorReason<object>> Reasons { get; }
    public string Code { get; set; }
    public string Message { get; set;}
    public int? StatusCode { get; }

    
}

public class ValidationErrorReason<TValue>
{
    public string PropertyName { get; }
    public string ErrorMessage { get; }
    public TValue AttemptedValue { get; }
    
    public ValidationErrorReason(
        string propertyName,
        string errorMessage,
        TValue attemptedValue)
    {
        PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        AttemptedValue = attemptedValue;
    }
}

public static class ValidationErrorExtensions
{
    public static ValidationError ExtractToList(this IEnumerable<ValidationFailure> validationFailures) 
    {
        var failuresList = validationFailures?.ToList() ?? new List<ValidationFailure>();

        if (failuresList.Count == 0)
        {
            return new ValidationError(
                "ValidationError",
                "No validation errors found.",
                new List<ValidationErrorReason<object>>(),
                StatusCodes.Status422UnprocessableEntity
            );
        }

        return new ValidationError(
            "ValidationError",
            "Validation error occurred.",
            failuresList.Select(failure => new ValidationErrorReason<object>(
                failure.PropertyName,
                failure.ErrorMessage,
                failure.AttemptedValue
            )).ToList(), // Ensures immediate execution instead of deferred
            StatusCodes.Status422UnprocessableEntity
        );
    }

    public static ValidationError WithEndpoint(this ValidationError validationFailure, string endpoint)
    {
        validationFailure.Message = $"Validation error occurred at endpoint{endpoint}";
        return validationFailure;
    }
}