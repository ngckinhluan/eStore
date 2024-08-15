using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;
using Repositories.Implementation;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implementation;

public class MemberService(IMemberRepository memberRepository, IMapper mapper) : IMemberService
{
    private IMemberRepository MemberRepository { get; } = memberRepository;
    private IMapper Mapper { get; } = mapper;

    public Task<Member?> AddAsync(MemberRequestDto entity)
    {
        var member = Mapper.Map<Member>(entity);
        return MemberRepository.AddAsync(member);
    }

    public async Task<Member?> UpdateAsync(int id, MemberRequestDto entity)
    {
        var member = Mapper.Map<Member>(entity);
        return await MemberRepository.UpdateAsync(id, member);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await MemberRepository.DeleteAsync(id);
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await MemberRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Member>?> GetAllAsync()
    {
        return await MemberRepository.GetAllAsync();
    }
}