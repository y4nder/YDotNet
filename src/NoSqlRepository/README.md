# 🎉 NoSql Generic Repository Pattern (MongoDB) 

🚀 **NoSqlRepository** is a super flexible and easy-to-use generic repository for MongoDB in .NET! It offers all the CRUD operations, smooth pagination, and even comes with an extension method to make your MongoDB configuration a breeze. 😎

## 🌟 Features

- **CRUD Operations**: 📝 Create, Read, Update, and Delete documents with ease.
- **Pagination**: 🧑‍💻 Seamlessly paginate your read operations to fetch data in chunks.
- **MongoDB Configuration Extension**: 🔧 Simplify your MongoDB setup with our extension method.
- **Super Flexible**: 💪 Works with any type of entity, just like magic.

## 🛠️ Installation

To install **NoSqlRepository**, simply run the command below in your **Package Manager Console**:

```bash
Install-Package YDotNet.NoSqlRepository
```

Or, if you prefer using the **.NET CLI** (for that command line power 💥):

```bash
dotnet add package YDotNet.NoSqlRepository
```

## ⚙️ Setup

### 1. Add MongoDB Configuration

Add your MongoDB connection settings in `appsettings.json` (or `appsettings.Development.json` for development purposes):

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "YourDatabaseName"
  }
}
```

### 2. Register MongoDB Services in `Program.cs`

In `Program.cs` (or `Startup.cs` for the legacy folks), register MongoDB services:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NoSqlRepository;
using MongoDB.Driver;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register MongoDb services with the extension method
        builder.Services.AddMongoDbExtensions(builder.Configuration);

        // Register the repository
        builder.Services.AddScoped(typeof(NoSqlRepository<>));

        var app = builder.Build();
        app.Run();
    }
}
```

## 🔧 Methods

Here’s a list of all the cool things you can do with **NoSqlRepository**! 😍

### `GetAllAsync()`

Fetches **all** documents from the MongoDB collection.

```csharp
public async Task<IEnumerable<T>> GetAllAsync()
```

### `FindAsync(FilterDefinition<T>? filter, int skip = 0, int limit = 10)`

Fetches documents based on a filter, with **pagination** support! 📜

```csharp
public async Task<IEnumerable<T>> FindAsync(FilterDefinition<T>? filter, int skip = 0, int limit = 10)
```

### `GetByIdAsync(string id)`

Fetch a document by its **ID**. 🕵️‍♂️

```csharp
public async Task<T?> GetByIdAsync(string id)
```

### `AddAsync(T entity)`

Add a brand-new document into your collection! 🎉

```csharp
public async Task AddAsync(T entity)
```

### `UpdateAsync(string id, T entity)`

Update an existing document by its **ID**. 🔄

```csharp
public async Task UpdateAsync(string id, T entity)
```

### `UpdateAsync(string id, UpdateDefinition<T> update)`

Apply a **partial update** to a document. 🛠️

```csharp
public async Task<UpdateResult> UpdateAsync(string id, UpdateDefinition<T> update)
```

### `DeleteAsync(string id)`

Goodbye, document! 👋

```csharp
public async Task DeleteAsync(string id)
```

## 🌱 Example Usage

### 🏷️ Example Entity

Let's create a simple entity for fun!

```csharp
public class YourEntity
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
}
```

### 🏗️ Creating the Repository

Make your custom repository extend the abstract `NoSqlRepository<T>` class. Here's how you can build your own repository for the entity:

```csharp
public class YourEntityRepository : NoSqlRepository<YourEntity>
{
    public YourEntityRepository(MongoClient client, IOptions<MongoDbSettings> settings, string? collectionName = null) 
        : base(client, settings, collectionName) { }
}
```

### 🛠️ Using the Repository

Now let’s see how you can inject and use the repository in your service:

```csharp
public class YourService
{
    private readonly YourEntityRepository _repository;

    public YourService(YourEntityRepository repository)
    {
        _repository = repository;
    }

    // Fetch all entities from the MongoDB collection
    public async Task GetAllEntitiesAsync()
    {
        var entities = await _repository.GetAllAsync();
        // Do something with the entities, like logging them
        Console.WriteLine(entities);
    }

    // Create a new entity and add it to the collection
    public async Task CreateNewEntityAsync()
    {
        var newEntity = new YourEntity { Name = "New Entity" };
        await _repository.AddAsync(newEntity);
    }
}
```

Here's the updated README with an additional usage section for the `RestaurantRepository` example, demonstrating how to access MongoDB collections and perform more extensive operations using the `Collection` instance:

---

## 🍽️ Custom Repository Example

### 🚀 Example: `RestaurantRepository`

If you want to do some custom queries or more advanced operations, you can easily extend **NoSqlRepository** and use the **protected `Collection`** instance for direct access to the MongoDB collection. Here's how you can create a custom repository for a `Restaurant` entity!

```csharp
public class RestaurantRepository : NoSqlRepository<Restaurant>
{
    public RestaurantRepository(MongoClient client, IOptions<MongoDbSettings> settings, string? collectionName = null) 
        : base(client, settings, collectionName)
    { }

    // Custom method to fetch restaurant names with pagination
    public async Task<IEnumerable<string>> GetRestaurantsAsync()
    {
        // Define filter (Empty means no filter, so we get all restaurants)
        var filter = Builders<Restaurant>.Filter.Empty;

        // Define projection to only return restaurant names
        var projection = Builders<Restaurant>.Projection.Expression(r => r.Name);

        // Perform query with pagination: skip 0, limit to 10 results
        var restaurantNames = await Collection
            .Find(filter)
            .Project(projection)  // Apply projection to get only the 'Name' field
            .Skip(0)               // Skip the first 0 records (pagination starting point)
            .Limit(10)             // Limit to 10 records
            .ToListAsync();        // Execute the query asynchronously

        // Return the list of restaurant names
        return restaurantNames;
    }
}
```

### 🔑 How It Works

- **Custom Queries**: Use the `Collection` property (inherited from `NoSqlRepository`) for any custom query operations like filtering, projections, and sorting. In this case, the `GetRestaurantsAsync()` method retrieves the restaurant names with pagination support.

- **Projection**: The method applies a **projection** to only retrieve the `Name` field from the `Restaurant` documents, keeping your queries efficient by avoiding unnecessary fields.

- **Pagination**: We use `.Skip(0)` and `.Limit(10)` to implement basic pagination. You can adjust the `skip` and `limit` values dynamically for more advanced pagination logic.

### 🧑‍🍳 Example Entity: `Restaurant`

For this example, the `Restaurant` entity might look like this:

```csharp
public class Restaurant
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Cuisine { get; set; }
    public bool IsOpen { get; set; }
}
```

### 🛠️ Usage

Once your repository is ready, you can use it in your service class or anywhere you need it. Here’s how you can inject and use your custom `RestaurantRepository`:

```csharp
public class RestaurantService
{
    private readonly RestaurantRepository _restaurantRepository;

    public RestaurantService(RestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    // Example method to fetch restaurant names
    public async Task GetRestaurantNamesAsync()
    {
        var restaurantNames = await _restaurantRepository.GetRestaurantsAsync();
        // Do something with the restaurant names, like logging or processing
        Console.WriteLine(string.Join(", ", restaurantNames));
    }
}
```

---

## 🛠️ Configuration Options

- **ConnectionString**: The URL to your MongoDB instance (e.g., `mongodb://localhost:27017`).
- **DatabaseName**: The name of the MongoDB database you want to use. 📂

## 📜 License

This project is licensed under the **MIT License**. Enjoy and have fun with MongoDB! 🎉

