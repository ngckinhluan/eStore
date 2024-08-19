using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessObjects.Context;
using BusinessObjects.Entities;
using DAOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace UnitTests.DAOsTests
{
    public class MemberDaoTest
    {
        private readonly Mock<DbSet<Member>> _mockSet;
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly List<Member> _memberList;

        public MemberDaoTest()
        {
            _mockSet = new Mock<DbSet<Member>>();
            _mockContext = new Mock<ApplicationDbContext>();
            _memberList = new List<Member>
            {
                new Member
                {
                    MemberId = 1, Email = "john.doe@gmail.com", Password = "password", City = "Anytown",
                    Country = "USA", CompanyName = "ABC Company"
                },
                new Member
                {
                    MemberId = 2, Email = "test@gmail.com", Password = "password123", City = "Ho Chi Minh",
                    Country = "Vietnam", CompanyName = "Amazing Tech"
                },
                new Member
                {
                    MemberId = 3, Email = "thong.nt@gmail.com", Password = "password456", City = "Ho Chi Minh",
                    Country = "Vietnam", CompanyName = "FPT Software"
                }
            };
            var queryable = _memberList.AsQueryable();
            _mockSet.As<IQueryable<Member>>().Setup(m => m.Provider).Returns(queryable.Provider);
            _mockSet.As<IQueryable<Member>>().Setup(m => m.Expression).Returns(queryable.Expression);
            _mockSet.As<IQueryable<Member>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            _mockSet.As<IQueryable<Member>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            _mockSet.As<IAsyncEnumerable<Member>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Member>(queryable.GetEnumerator()));

            _mockContext.Setup(c => c.Members).Returns(_mockSet.Object);

            // Delete method
            _mockSet.Setup(m => m.Remove(It.IsAny<Member>())).Callback<Member>(m => _memberList.Remove(m));
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => 1);

            // FindAsync method
            _mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => _memberList.SingleOrDefault(a => a.MemberId == (int)ids[0]));

            // Update method
            _mockSet.Setup(m => m.Update(It.IsAny<Member>())).Callback<Member>(updatedMember =>
            {
                var existingMember = _memberList.SingleOrDefault(a => a.MemberId == updatedMember.MemberId);
                if (existingMember != null)
                {
                    existingMember.Email = updatedMember.Email;
                    existingMember.Password = updatedMember.Password;
                    existingMember.City = updatedMember.City;
                    existingMember.Country = updatedMember.Country;
                    existingMember.CompanyName = updatedMember.CompanyName;
                }
            });
        }

        [Fact]
        public async Task GetMembers_ReturnsAllMembers()
        {
            var memberDao = new MemberDao(_mockContext.Object);
            var result = await memberDao.GetMembers();
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetMemberById_ReturnsMember()
        {
            var memberDao = new MemberDao(_mockContext.Object);
            var result = await memberDao.GetMemberById(1);
            Assert.NotNull(result);
            Assert.Equal("john.doe@gmail.com", result?.Email);
        }

        [Fact]
        public async Task AddMember_AddsMember()
        {
            var newMember = new Member
            {
                MemberId = 4, Email = "new.member@gmail.com", Password = "newpassword", City = "New City",
                Country = "New Country", CompanyName = "New Company"
            };
            var memberDao = new MemberDao(_mockContext.Object);
            await memberDao.AddMember(newMember);
            _mockSet.Verify(m => m.AddAsync(newMember, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMember_UpdatesMember()
        {
            var updatedMember = new Member
            {
                MemberId = 1, Email = "updated.email@gmail.com", Password = "updatedpassword", City = "Updated City",
                Country = "Updated Country", CompanyName = "Updated Company"
            };
            var memberDao = new MemberDao(_mockContext.Object);
            var result = await memberDao.UpdateMember(1, updatedMember);
            Assert.NotNull(result);
            Assert.Equal("updated.email@gmail.com", result?.Email);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteMember_DeletesMember()
        {
            var memberDao = new MemberDao(_mockContext.Object);
            var result = await memberDao.DeleteMember(1);
            Assert.Equal(1, result);
            _mockSet.Verify(m => m.Remove(It.IsAny<Member>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}