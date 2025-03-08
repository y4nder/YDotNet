namespace UnitOfWork;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
