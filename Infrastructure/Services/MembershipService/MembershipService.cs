using System.Net;
using Domain.DTOs.MembershipDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.MembershipService;

public class MembershipService(DataContext context, IFileService fileService) : IMembershipService
{
    public async Task<PagedResponse<List<GetMembershipDto>>> GetMembershipsAsync(MembershipFilter filter)
    {
        try
        {
            var Memberships = context.Memberships.Include(x => x.User).AsQueryable();

            if (filter.Price != null)
                Memberships = Memberships.Where(x => x.Price == filter.Price);


            var response = await Memberships.Select(x => new GetMembershipDto()
            {
                Type = x.Type,
                EndDate = x.EndDate,
                StartDate = x.StartDate,
                UserId = x.UserId,
                Price = x.Price,
                Photo = x.Photo,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var totalRecord = await Memberships.CountAsync();

            return new PagedResponse<List<GetMembershipDto>>(response, totalRecord, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetMembershipDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }



    public async Task<Response<GetMembershipDto>> GetMembershipByIdAsync(int MembershipId)
    {
        try
        {
            var existing = await context.Memberships.FirstOrDefaultAsync(x => x.Id == MembershipId);
            if (existing == null) return new Response<GetMembershipDto>(HttpStatusCode.BadRequest, "Not Found");
            var response = new GetMembershipDto()
            {
                Type = existing.Type,
                EndDate = existing.EndDate,
                StartDate = existing.StartDate,
                UserId = existing.UserId,
                Price = existing.Price,
                Photo = existing.Photo,
                Id = existing.Id,
            };
            return new Response<GetMembershipDto>(response);
        }
        catch (Exception e)
        {
            return new Response<GetMembershipDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }



    public async Task<Response<string>> CreateMembershipAsync(CreateMembershipDto createMembership)
    {
        try
        {
            var Membership = new Membership()
            {
                Type = createMembership.Type,
                EndDate = createMembership.EndDate,
                StartDate = createMembership.StartDate,
                UserId = createMembership.UserId,
                Price = createMembership.Price,
                Photo = await fileService.CreateFile(createMembership.Photo),
            };
            await context.Memberships.AddAsync(Membership);
            await context.SaveChangesAsync();
            return new Response<string>("Successfully created Membership");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }



    public async Task<Response<string>> UpdateMembershipAsync(UpdateMembershipDto updateMembership)
    {
        try
        {
            var existingMembership = await context.Memberships.FirstOrDefaultAsync(x => x.Id == updateMembership.Id);
            if (existingMembership == null) return new Response<string>(HttpStatusCode.BadRequest, "Membership not found");

            if (updateMembership.Photo != null)
            {
                if (existingMembership.Photo != null) fileService.DeleteFile(existingMembership.Photo);
                existingMembership.Photo = await fileService.CreateFile(updateMembership.Photo);
            }

            existingMembership.StartDate = updateMembership.StartDate;
            existingMembership.EndDate = updateMembership.EndDate;
            existingMembership.Price = updateMembership.Price;
            existingMembership.Type = updateMembership.Type;
            existingMembership.UserId = updateMembership.UserId;
            if (updateMembership.Photo != null)
                existingMembership.Photo = await fileService.CreateFile(updateMembership.Photo);

            await context.SaveChangesAsync();
            return new Response<string>("Successfully updated the Membership");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }



    public async Task<Response<bool>> DeleteMembershipAsync(int MembershipId)
    {
        try
        {
            var existing = await context.Memberships.FirstOrDefaultAsync(x => x.Id == MembershipId);
            if (existing == null) return new Response<bool>(HttpStatusCode.BadRequest, "Not Found");
            if (existing.Photo != null) fileService.DeleteFile(existing.Photo);
            context.Memberships.Remove(existing);
            await context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    
}
