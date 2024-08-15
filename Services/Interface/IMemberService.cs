using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;

namespace Services.Interface;

public interface IMemberService
{
    Task<Member?> AddAsync(MemberRequestDto entity);
    Task<Member?> UpdateAsync(int id, MemberRequestDto entity);
    Task<int> DeleteAsync(int id);
    Task<Member?> GetByIdAsync(int id);
    Task<IEnumerable<Member>?> GetAllAsync();
}