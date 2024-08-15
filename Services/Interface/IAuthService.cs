using BusinessObjects.Entities;

namespace Services.Interface;

public interface IAuthService
{ 
    public Task<Member?> Login(string email, string password);
    public Task<Member?> Register(Member member);
}