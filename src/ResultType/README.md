# ResultType

## Overview
`ResultType` is a lightweight C# library designed to facilitate structured handling of operation results in .NET applications. It provides a `Result` type that encapsulates success or failure outcomes, along with error details, and includes ASP.NET Core integration for seamless API response handling.

## Features
- Strongly-typed `Result` and `Result<T>` for representing success or failure states.
- Built-in error handling with `IError`, `Error`, and `ValidationError`.
- ASP.NET Core minimal API integration via `ToHttpResult()`.
- Utility methods like `Match()` for streamlined success/failure handling.

## Installation

You can add `ResultType` to your .NET project using:

```sh
// Install via .NET CLI to upload
 
```

## Usage

### Basic Success and Failure Handling
```csharp
using ResultType;
using ResultType.Errors;

Result successResult = Result.Success();
Result failureResult = Result.Failure(new Error("NotFound", "Item not found", 404));

if (failureResult.IsFailure)
{
    Console.WriteLine($"Error: {failureResult.Error?.Message}");
}
```



### Working with Generic Results
```csharp
var successWithValue = Result.Success("Hello, World!");
var failureWithValue = Result.Failure<string>(new Error("InvalidData", "The provided data is incorrect.", 400));

if (successWithValue.IsSuccess)
{
    Console.WriteLine(successWithValue.Value); // Outputs: Hello, World!
}
```

### Working with Http Results
```csharp
var successWithValue = Result.Success("Hello, World!");
var failureWithValue = Result.Failure<string>(new Error("InvalidData", "The provided data is incorrect.", 400));

IResult success = successWithValue.toHttpResult();
IResult failure = failureWithValue.toHttpResult();
```

### Implicit Conversion
```csharp
Result<string> result = "Implicit success"; // Automatically wraps value in Result.Success
```

## ASP.NET Core Integration

### Returning `Result` in Minimal APIs
```csharp
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResultType;
using ResultType.Errors;

var app = WebApplication.Create();

app.MapGet("/user/{id}", (int id) =>
{
    if (id <= 0)
        return Result.Failure(new Error("InvalidId", "User ID must be positive", 400)).ToHttpResult();

    return Result.Success(new { Id = id, Name = "John Doe" }).ToHttpResult();
});

app.Run();
```

## Error Handling

### Custom Error Handling
```csharp
Error error = new Error("Forbidden", "You are not authorized to access this resource.", 403);
Result failure = Result.Failure(error);
```

## License
This project is licensed under the MIT License.

## Contributing
Contributions are welcome! Feel free to open issues or submit pull requests.

---



