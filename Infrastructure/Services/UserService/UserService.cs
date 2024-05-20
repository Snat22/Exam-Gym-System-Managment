using System.Net;
using System.Security.Cryptography;
using System.Text;
using Domain.DTOs.UserDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.UserService;

public class UserService(DataContext context, UserManager<IdentityUser> _userManager) : IUserService
{

    public async Task<Response<string>> RegisterUser(CreateUserDto addUserDto)
    {
        try
        {
            var existing = await context.Users.FirstOrDefaultAsync(x => x.Name == addUserDto.Name && x.HashPassword == ConvertToHash(addUserDto.Password));
            if (existing != null) return new Response<string>(HttpStatusCode.BadRequest, "User already exists");
            var user = new User()
            {
                Name = addUserDto.Name,
                Email = addUserDto.Email,
                HashPassword = ConvertToHash(addUserDto.Password),
                RegisterDate = addUserDto.RegisterDate,
                Role = addUserDto.Role,
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return new Response<string>("User added successfully");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }



    public async Task<PagedResponse<List<GetUserDto>>> GetUserAsync(UserFilter filter)
    {
        try
        {
            var query = context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));

            var user = await query.Select(x => new GetUserDto()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                HashPassword = x.HashPassword,
                RegisterDate = x.RegisterDate,
                Role = x.Role,
            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var totalRecord = await query.CountAsync();
            return new PagedResponse<List<GetUserDto>>(user, totalRecord, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetUserDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }



    public async Task<Response<bool>> DeleteUserAsync(int id)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return new Response<bool>(HttpStatusCode.NotFound, "User Not Found");

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return new Response<bool>(HttpStatusCode.OK, "User deleted succesfuly");
        }
        catch (Exception ex)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }



    public async Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        try
        {
            var request = await context.Users.FirstOrDefaultAsync(e => e.Id == updateUserDto.Id);
            if (request == null) return new Response<string>(HttpStatusCode.NotFound, "User Not Found");

            request.Id = updateUserDto.Id;
            request.Name = updateUserDto.Name;
            request.Email = updateUserDto.Email;
            request.HashPassword = ConvertToHash(updateUserDto.HashPassword);
            request.RegisterDate = updateUserDto.RegisterDate;
            request.Role = updateUserDto.Role;

            await context.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "User Updated Successfuly");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }



    public async Task<Response<GetUserDto>> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(e => e.Id == id);
            if (user == null) return new Response<GetUserDto>(HttpStatusCode.NotFound, "User Not Found");
            var result = new GetUserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                HashPassword = user.HashPassword,
                Role = user.Role,
                RegisterDate = user.RegisterDate,
            };
            return new Response<GetUserDto>(result);
        }
        catch (Exception e)
        {
            return new Response<GetUserDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }



    private static string ConvertToHash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

}