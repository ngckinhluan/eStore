using BusinessObjects.Context;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Interface;

namespace Services.Implementation;

public class AuthService(ApplicationDbContext context) : IAuthService
{
    private ApplicationDbContext Context { get; } = context;

    public async Task<Member?> Login(string email, string password)
    {
        var member = await Context.Members.FirstOrDefaultAsync(m => m.Email == email && m.Password == password);
        return member;
    }

    public async Task<Member?> Register(Member member)
    {
        await Context.AddAsync(member);
        await Context.SaveChangesAsync();
        return member;
    }
}