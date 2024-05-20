using Domain.DTOs.MembershipDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.MembershipService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembershipController(IMembershipService MembershipService) : ControllerBase
{
    [HttpGet]
    public async Task<Response<List<GetMembershipDto>>> GetMembershipsAsync([FromQuery] MembershipFilter MembershipFilter)
        => await MembershipService.GetMembershipsAsync(MembershipFilter);

    [HttpGet("{MembershipId:int}")]
    public async Task<Response<GetMembershipDto>> GetMembershipByIdAsync(int MembershipId)
        => await MembershipService.GetMembershipByIdAsync(MembershipId);

    [HttpPost("create")]
    public async Task<Response<string>> CreateMembershipAsync([FromForm] CreateMembershipDto Membership)
        => await MembershipService.CreateMembershipAsync(Membership);


    [HttpPut("update")]
    public async Task<Response<string>> UpdateMembershipAsync([FromForm] UpdateMembershipDto Membership)
        => await MembershipService.UpdateMembershipAsync(Membership);

    [HttpDelete("{MembershipId:int}")]
    public async Task<Response<bool>> DeleteMembershipAsync(int MembershipId)
        => await MembershipService.DeleteMembershipAsync(MembershipId);
}
