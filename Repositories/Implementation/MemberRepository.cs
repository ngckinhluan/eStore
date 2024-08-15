using BusinessObjects.Entities;
using DAOs;
using Repositories.Interface;

namespace Repositories.Implementation;

public class MemberRepository(MemberDao memberDao) : IMemberRepository
{
    private MemberDao MemberDao { get; } = memberDao;
    
    public async Task<Member?> AddAsync(Member entity)
    {
        return await MemberDao.AddMember(entity);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await MemberDao.DeleteMember(id);
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await MemberDao.GetMemberById(id);
    }

    public async Task<IEnumerable<Member>?> GetAllAsync()
    {
        return await MemberDao.GetMembers();
    }

    public async Task<Member?> UpdateAsync(int id, Member entity)
    {
        return await MemberDao.UpdateMember(id, entity);
    }
}