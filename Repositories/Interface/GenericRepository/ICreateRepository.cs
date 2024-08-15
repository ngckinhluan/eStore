namespace Repositories.Interface.GenericRepository;

public interface ICreateRepository<T> where T : class
{
    Task<T?> AddAsync(T entity);
}