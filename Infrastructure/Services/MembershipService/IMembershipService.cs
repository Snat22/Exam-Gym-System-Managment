using Domain.DTOs.MembershipDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.MembershipService;

public interface IMembershipService
{
    Task<PagedResponse<List<GetMembershipDto>>> GetMembershipsAsync(MembershipFilter filter);
    Task<Response<GetMembershipDto>> GetMembershipByIdAsync(int MembershipId);
    Task<Response<string>> CreateMembershipAsync(CreateMembershipDto createMembership);
    Task<Response<string>> UpdateMembershipAsync(UpdateMembershipDto updateMembership);
    Task<Response<bool>> DeleteMembershipAsync(int MembershipId);
}
