namespace Repositories.Interface.GenericRepository;

public interface IDeleteRepository<T> where T : class
{
    Task<int> DeleteAsync(int id);
}