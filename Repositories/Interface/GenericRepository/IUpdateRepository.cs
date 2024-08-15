namespace Repositories.Interface.GenericRepository;

public interface IUpdateRepository<T> where T : class
{
    Task<T?> UpdateAsync(int id, T entity);
}