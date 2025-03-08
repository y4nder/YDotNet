# 🔄 Unit of Work Pattern for Entity Framework Core  

This library provides a **simple and efficient implementation** of the **Unit of Work** pattern for **Entity Framework Core** applications. Inspired by best practices, this ensures that all **database operations** within a business transaction are handled **consistently and atomically**.  

## ✨ Features  

✅ **Clean abstraction** over Entity Framework Core's `DbContext`  
✅ **Async save operations** for better performance  
✅ **Seamless integration** with dependency injection  
✅ **Supports any EF Core DbContext**  

## 📦 Installation  

Add the package to your project:  

```bash
dotnet add package Yander.UnitOfWork
```

## 🚀 Usage  

### 1️⃣ **Setup**  

Register the **Unit of Work** with your `DbContext` in `Program.cs`:  

```csharp
builder.Services.AddUnitOfWork<AppDbContext>();
```

### 2️⃣ **Dependency Injection**  

Inject `IUnitOfWork` into your services or controllers:  

```csharp
public class MyService
{
    private readonly IUnitOfWork _unitOfWork;

    public MyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
```

### 3️⃣ **Using the Unit of Work**  

```csharp
public class MyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DbContext _context;

    public MyService(IUnitOfWork unitOfWork, DbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task SaveDataAsync()
    {
        // Perform database operations
        _context.Users.Add(new User { Name = "John" });
        
        // Save all changes atomically
        await _unitOfWork.SaveChangesAsync();
    }
}
```

## 🔧 Requirements  

- 🟢 **.NET 6.0 or higher**  
- 🟢 **Entity Framework Core 6.0 or higher**  

## 📜 License  

📝 **MIT License** – Free to use and modify!  

## 🤝 Contributing  

Contributions are **welcome**! 🚀 Feel free to submit issues, enhancements, or pull requests.  

