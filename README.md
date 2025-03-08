# YDotNet

Yander is a comprehensive .NET library that provides essential building blocks for building robust and maintainable applications. It consists of three main components that work together to improve your application's architecture and error handling.

## Components

### 1. ResultType

A robust implementation of the Result pattern for better error handling in C#. This component helps you:
- Handle errors without exceptions
- Chain operations with fluent syntax
- Maintain type safety throughout your error handling
- Separate success and failure paths clearly

[📖 View ResultType Documentation](/src/ResultType/README.md)

### 2. Repository

A generic repository pattern implementation that provides:
- Standard CRUD operations
- Type-safe entity handling
- Flexible data access abstraction
- Easy integration with various data sources
- Built-in support for entity interfaces

[📖 View Repository Documentation](/src/Repository/README.md)

### 3. UnitOfWork

An implementation of the Unit of Work pattern that offers:
- Transaction management
- Atomic operations
- Consistent data state
- Integration with the Repository pattern
- Simplified data persistence

[📖 View UnitOfWork Documentation](/src/UnitOfWork/README.md)

## Installation

You can install each component separately using NuGet:

```bash
dotnet add package Yander.ResultType
dotnet add package Yander.Repository
dotnet add package Yander.UnitOfWork
```

## Features

- 🚀 Modern C# features
- 💪 Type-safe operations
- 🧩 Modular design
- 📦 Easy integration
- 🔧 Extensible architecture
- 📝 Comprehensive documentation

## Requirements

- .NET 6.0 or later
- C# 10.0 or later

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

If you encounter any issues or need support, please file an issue on the GitHub repository.
