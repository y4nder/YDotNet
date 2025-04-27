using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace NoSqlRepository;

public abstract class NoSqlRepository<T>
{
    protected readonly IMongoCollection<T> Collection;
    
    public NoSqlRepository(MongoClient client, IOptions<MongoDbSettings> settings, string? collectionName = null)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        var name = collectionName ?? typeof(T).Name + "s";
        Collection = database.GetCollection<T>(name.ToLower());
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Collection.Find(_ => true).ToListAsync();
    }
    
    public async Task<IEnumerable<T>> FindAsync(FilterDefinition<T>? filter, int skip = 0, int limit = 10)
    {
        filter ??= Builders<T>.Filter.Empty;
        return await Collection.Find(filter).Skip(skip).Limit(limit).ToListAsync();
    }
    
    public async Task<T?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId)) return default;
        return await Collection.Find(Builders<T>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
    }

    public async Task AddAsync(T entity)
    {
        await Collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(string id, T entity)
    {
        if (!ObjectId.TryParse(id, out var objectId)) return;
        await Collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", objectId), entity);
    }

    public async Task<UpdateResult> UpdateAsync(string id, UpdateDefinition<T> update)
    {
        if (!ObjectId.TryParse(id, out var objectId)) return default!;
        return await Collection.UpdateOneAsync(
            Builders<T>.Filter.Eq("_id", objectId), update
        );
    }

    public async Task DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId)) return;
        await Collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", objectId));
    }
}