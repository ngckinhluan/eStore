using BusinessObjects.Entities;
using Repositories.Interface.GenericRepository;

namespace Repositories.Interface;

public interface IMemberRepository : ICreateRepository<Member>, IDeleteRepository<Member>, IReadRepository<Member>, IUpdateRepository<Member>
{
    
}