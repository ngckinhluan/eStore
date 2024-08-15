using BusinessObjects.Entities;
using Repositories.Interface.GenericRepository;

namespace Repositories.Interface;

public interface ICategoryRepository : ICreateRepository<Category>, IDeleteRepository<Category>, IReadRepository<Category>, IUpdateRepository<Category>
{
    
}