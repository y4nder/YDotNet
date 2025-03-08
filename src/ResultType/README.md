# 🚀 ResultType

## 🌟 Overview
**ResultType** is a lightweight and powerful C# library designed to streamline structured handling of operation results in .NET applications. It provides a `Result` type that encapsulates success or failure outcomes, along with detailed error information. With built-in ASP.NET Core integration, it simplifies API response handling effortlessly.

🔗 **NuGet Package:** [YDotNet.ResultType](https://www.nuget.org/packages/YDotNet.ResultType/)

---

## ✨ Features
✅ Strongly-typed `Result` and `Result<T>` for clear success/failure handling.  
✅ Built-in error management with `IError`, `Error`, and `ValidationError`.  
✅ **ASP.NET Core** integration with `ToHttpResult()` for easy API responses.  
✅ Utility methods like `Match()` for streamlined handling.  
✅ Implicit conversions for clean and concise code.  

---

## 📦 Installation

Add **ResultType** to your .NET project using:

```sh
 dotnet add package YDotNet.ResultType
```

---

## 🚀 Quick Start

### ✅ Basic Success & Failure Handling
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

### 🔹 Handling Generic Results
```csharp
var successWithValue = Result.Success("Hello, World!");
var failureWithValue = Result.Failure<string>(new Error("InvalidData", "The provided data is incorrect.", 400));

if (successWithValue.IsSuccess)
{
    Console.WriteLine(successWithValue.Value); // Outputs: Hello, World!
}
```

### 🌐 Converting to HTTP Results (ASP.NET Core)
```csharp
IResult success = Result.Success("Hello, World!").ToHttpResult();
IResult failure = Result.Failure<string>(new Error("InvalidData", "The provided data is incorrect.", 400)).ToHttpResult();
```

### 🔄 Implicit Conversion
```csharp
Result<string> result = "Implicit success"; // Automatically wraps value in Result.Success
```

---

## 🌍 ASP.NET Core Integration

### 🏗️ Using `Result` in Minimal APIs
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

---

## ⚠️ Error Handling

### 🛠️ Custom Error Handling
```csharp
Error error = new Error("Forbidden", "You are not authorized to access this resource.", 403);
Result failure = Result.Failure(error);
```

---

## 📜 License
This project is licensed under the **MIT License**.

---

## 🤝 Contributing
We welcome contributions! Feel free to **open issues** or **submit pull requests**.


