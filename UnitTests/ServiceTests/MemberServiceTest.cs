using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;
using Moq;
using Repositories.Interface;
using Services.Implementation;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class MemberServiceTest
    {
        private readonly Mock<IMemberRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MemberService _memberService;
        private readonly List<Member> _memberList;

        public MemberServiceTest()
        {
            _mockRepository = new Mock<IMemberRepository>();
            _mockMapper = new Mock<IMapper>();
            _memberService = new MemberService(_mockRepository.Object, _mockMapper.Object);

            _memberList = new List<Member>
            {
                new Member { MemberId = 1, CompanyName = "Member 1" },
                new Member { MemberId = 2, CompanyName = "Member 2" }
            };
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllMembers()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_memberList);

            // Act
            var result = await _memberService.GetAllAsync();

            // Assert
            Assert.Equal(_memberList.Count, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMember()
        {
            // Arrange
            var member = _memberList[0];
            _mockRepository.Setup(repo => repo.GetByIdAsync(member.MemberId)).ReturnsAsync(member);

            // Act
            var result = await _memberService.GetByIdAsync(member.MemberId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(member.MemberId, result.MemberId);
        }

        [Fact]
        public async Task AddAsync_AddsMember()
        {
            // Arrange
            var memberRequest = new MemberRequestDto { CompanyName = "Member 3" };
            var member = new Member { MemberId = 3, CompanyName = memberRequest.CompanyName };
            
            _mockMapper.Setup(m => m.Map<Member>(memberRequest)).Returns(member);
            _mockRepository.Setup(repo => repo.AddAsync(member)).ReturnsAsync(member);

            // Act
            var result = await _memberService.AddAsync(memberRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(member.CompanyName, result.CompanyName);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesMember()
        {
            // Arrange
            var memberRequest = new MemberRequestDto { CompanyName = "Updated Member 1" };
            var updatedMember = new Member { MemberId = 1, CompanyName = memberRequest.CompanyName };

            _mockMapper.Setup(m => m.Map<Member>(memberRequest)).Returns(updatedMember);
            _mockRepository.Setup(repo => repo.UpdateAsync(updatedMember.MemberId, updatedMember)).ReturnsAsync(updatedMember);

            // Act
            var result = await _memberService.UpdateAsync(updatedMember.MemberId, memberRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedMember.CompanyName, result.CompanyName);
        }

        [Fact]
        public async Task DeleteAsync_DeletesMember()
        {
            // Arrange
            var memberId = 1;
            _mockRepository.Setup(repo => repo.DeleteAsync(memberId)).ReturnsAsync(1);

            // Act
            var result = await _memberService.DeleteAsync(memberId);

            // Assert
            Assert.Equal(1, result);
        }
    }
}