using Microsoft.EntityFrameworkCore;

namespace Repository;

public abstract class Repository<TEntity, TIdentifier> where TEntity : class, IEntity
{
    private readonly DbContext _context;

    protected readonly DbSet<TEntity> DBSet;

    protected Repository(DbContext context)
    {
        _context = context;
        DBSet = _context.Set<TEntity>();
    }
    
    public async Task<TEntity?> GetByIdAsync(TIdentifier id) => await DBSet.FindAsync(id);

    public void Add(TEntity entity) => DBSet.Add(entity);
    

    public void Update(TEntity entity) => DBSet.Update(entity);
    

    public void Delete(TEntity entity) => DBSet.Remove(entity);

    public async Task<List<TEntity>> GetAllAsync() => await DBSet.ToListAsync();
}
