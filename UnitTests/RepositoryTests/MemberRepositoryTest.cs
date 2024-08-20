using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessObjects.Context;
using BusinessObjects.Entities;
using DAOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repositories.Implementation;
using Xunit;

namespace UnitTests.RepositoryTests
{
    public class MemberRepositoryTest
    {
        private readonly Mock<DbSet<Member>> _mockSet;
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly List<Member> _memberList;
        private readonly MemberRepository _memberRepository;

        public MemberRepositoryTest()
        {
            _mockSet = new Mock<DbSet<Member>>();
            _mockContext = new Mock<ApplicationDbContext>();
            _memberList = new List<Member>
            {
                new Member { MemberId = 2, Email = "johndoe@example.com", Password = "password123", City = "New York", Country = "USA", CompanyName = "Doe Enterprises" },
                new Member { MemberId = 3, Email = "janesmith@example.com", Password = "securePass456", City = "London", Country = "UK", CompanyName = "Smith Consulting" },
                new Member { MemberId = 4, Email = "alexjohnson@example.com", Password = "password789", City = "Sydney", Country = "Australia", CompanyName = "Johnson & Co" }
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

            var memberDao = new MemberDao(_mockContext.Object);
            _memberRepository = new MemberRepository(memberDao);

            // Delete method
            _mockSet.Setup(m => m.Remove(It.IsAny<Member>())).Callback<Member>(m => _memberList.Remove(m));
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            
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

            // Add method
            _mockSet.Setup(m => m.Add(It.IsAny<Member>())).Callback<Member>(m => _memberList.Add(m));
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllMembers()
        {
            var result = await _memberRepository.GetAllAsync();
            Assert.Equal(_memberList.Count, result.Count()); // avoid hardcoding the count
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMember()
        {
            var result = await _memberRepository.GetByIdAsync(2);
            Assert.NotNull(result);
            Assert.Equal("johndoe@example.com", result?.Email);
        }

        [Fact]
        public async Task AddAsync_AddsMember()
        {
            // Arrange
            var newMember = new Member
            {
                MemberId = 5,
                Email = "newmember@example.com",
                Password = "newpassword",
                City = "Berlin",
                Country = "Germany",
                CompanyName = "New Company"
            };

            // Act
            await _memberRepository.AddAsync(newMember);

            // Assert
            _mockSet.Verify(m => m.AddAsync(newMember, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesMember()
        {
            // Arrange
            var updatedMember = new Member
            {
                MemberId = 2,
                Email = "updatedemail@example.com",
                Password = "updatedpassword",
                City = "Updated City",
                Country = "Updated Country",
                CompanyName = "Updated Company"
            };

            // Act
            var result = await _memberRepository.UpdateAsync(updatedMember.MemberId, updatedMember);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedMember.Email, result?.Email);
        }

        [Fact]
        public async Task DeleteAsync_DeletesMember()
        {
            // Act
            var result = await _memberRepository.DeleteAsync(2);

            // Assert
            Assert.Equal(1, result);
            Assert.DoesNotContain(_memberList, m => m.MemberId == 2);
        }
    }
}