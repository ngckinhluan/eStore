using BusinessObjects.Context;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAOs;

public class MemberDao(ApplicationDbContext context)
{
    private ApplicationDbContext Context { get; } = context;
    public async Task<IEnumerable<Member>> GetMembers()
    {
        return await Context.Members.ToListAsync();
    }
    
    public async Task<Member?> GetMemberById(int id)
    {
        return await Context.Members.FindAsync(id);
    }
    
    public async Task<Member> AddMember(Member member)
    {
        await Context.Members.AddAsync(member);
        await Context.SaveChangesAsync();
        return member;
    }
    
    public async Task<Member?> UpdateMember(int id, Member member)
    {
        var existingMember = await Context.Members.FindAsync(id);
        if (existingMember == null) return null;
        existingMember.City = member.City;
        existingMember.Email = member.Email;
        existingMember.CompanyName = member.CompanyName;
        existingMember.Country = member.Country;
        existingMember.Password = member.Password;
        await Context.SaveChangesAsync();
        return existingMember;
    }

    
    public async Task<int> DeleteMember(int id)
    {
        var member = await Context.Members.FindAsync(id);
        if (member == null) return 0;
        Context.Members.Remove(member);
        return await Context.SaveChangesAsync();
    }
}