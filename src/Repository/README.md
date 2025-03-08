# 🚀 Generic Repository Pattern  

This project provides a **generic repository pattern** implementation for .NET applications using **Entity Framework Core**. Inspired by **Java Spring's repository approach**, this abstraction layer helps separate business logic from data access logic, making your application **more maintainable, scalable, and testable**.  



🔗 **NuGet Package:** [YDotNet.Repository](https://www.nuget.org/packages/YDotNet.Repository/)


## ✨ Features  

✅ Generic repository that works with **any entity type**  
✅ **Async/await support** for improved performance  
✅ **CRUD operations** (Create, Read, Update, Delete)  
✅ **Type-safe** operations with generic type parameters  
✅ Built on top of **Entity Framework Core**  

## 📦 Installation  

```bash
dotnet add package YDotNet.Repository
```

## 📌 Usage  

### 1️⃣ Define Your Entity  

Your entity class must implement the `IEntity` interface:  

```csharp
public class YourEntity : IEntity
{
    public int Id { get; set; }
    // ... other properties
}
```

### 2️⃣ Create a Repository  

Create a specific repository for your entity by inheriting from the base `Repository` class:  

```csharp
public class YourEntityRepository : Repository<YourEntity, int>
{
    public YourEntityRepository(DbContext context) : base(context)
    {
    }
}
```

### 3️⃣ Use the Repository  

```csharp
// In your service or controller:
public class YourService
{
    private readonly YourEntityRepository _repository;

    public YourService(YourEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task<YourEntity?> GetEntityById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<YourEntity>> GetAllEntities()
    {
        return await _repository.GetAllAsync();
    }
}
```

## 🔧 Available Methods  

- `GetByIdAsync(TIdentifier id)`: 🔍 Retrieves an entity by its ID  
- `Add(TEntity entity)`: ➕ Adds a new entity  
- `Update(TEntity entity)`: 🔄 Updates an existing entity  
- `Delete(TEntity entity)`: 🗑️ Removes an entity  
- `GetAllAsync()`: 📜 Retrieves all entities  

## 📌 Dependencies  

- **Microsoft.EntityFrameworkCore**  

## 🤝 Contributing  

Contributions are **welcome**! 🎉 Feel free to submit issues, bug fixes, or feature requests.  

## 📜 License  

📝 **MIT License** – Free to use and modify!  

