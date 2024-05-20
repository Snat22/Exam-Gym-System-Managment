using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.DTOs.AuthDto;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.AuthService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Authontication;

public class AuthService(DataContext context, IConfiguration configuration) : IAuthService
{

    public async Task<Response<string>> RegisterUser(RegisterDto registerDto)
    {
        try
        {
            var existing = await context.Users.FirstOrDefaultAsync(x => x.Name == registerDto.Name && x.HashPassword == ConvertToHash(registerDto.Password));
            if (existing != null) return new Response<string>(HttpStatusCode.BadRequest, "User already exists");
            var user = new User()
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                HashPassword = ConvertToHash(registerDto.Password),
                RegisterDate = DateTime.UtcNow,
                Role = "admin",
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

    
    public async Task<Response<string>> Login(LoginDto model)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Name == model.UserName && x.HashPassword == ConvertToHash(model.Password));
        if (user == null)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "user not found");
        }

        var password = ConvertToHash(model.Password);
        if (password == user.HashPassword) return new Response<string>(GenerateJwtToken(user));

        return new Response<string>(HttpStatusCode.BadRequest, "error occured");
    }





    private string GenerateJwtToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };
        //add roles

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        var securityTokenHandler = new JwtSecurityTokenHandler();
        var tokenString = securityTokenHandler.WriteToken(token);
        return tokenString;
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