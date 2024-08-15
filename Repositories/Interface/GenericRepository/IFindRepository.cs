namespace Repositories.Interface.GenericRepository;

public interface IFindRepository<T> where T : class
{
    Task<IEnumerable<T>> Find(Func<T, bool> predicate);
}