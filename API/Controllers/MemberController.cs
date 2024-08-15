using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.DTOs.Response;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace eStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberController(IMemberService memberService, IMapper mapper) : ControllerBase
{
    private IMemberService MemberService { get; } = memberService;
    private IMapper Mapper { get; } = mapper;
    
    [HttpGet("GetMembers")]
    public async Task<IActionResult> GetMembers()
    {
        var result = await MemberService.GetAllAsync();
        if (result == null)
        {
            return NotFound("No members found");
        }
        var response = Mapper.Map<IEnumerable<MemberResponseDto>>(result);
        return Ok(response);
    }
    
    [HttpGet("GetMemberById/{id}")]
    public async Task<IActionResult> GetMemberById(int id)
    {
        var result = await MemberService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound("Member not found");
        }
        var response = Mapper.Map<MemberResponseDto>(result);
        return Ok(response);
    }
    
    [HttpPost("AddMember")]
    public async Task<IActionResult> AddMember([FromBody] MemberRequestDto member)
    {
        var entity = Mapper.Map<Member>(member);
        var result = await MemberService.AddAsync(member);
        if (result == null)
        {
            return BadRequest("Failed to add member");
        }
        return Ok(result);
    }
    
    [HttpPut("UpdateMember/{id}")]
    public async Task<IActionResult> UpdateMember(int id, [FromBody] MemberRequestDto member)
    {
        var result = await MemberService.UpdateAsync(id, member);
        if (result == null)
        {
            return BadRequest("Failed to update member");
        }
        return Ok(result);
    }
    
    [HttpDelete("DeleteMember/{id}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        var result = await MemberService.DeleteAsync(id);
        if (result == 0)
        {
            return NotFound("Member not found");
        }
        return Ok("Member has been deleted");
    }
}