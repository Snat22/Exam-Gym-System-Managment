using System.Text;
using Infrastructure.Data;
using Infrastructure.Seed;
using Infrastructure.Services.Authontication;
using Infrastructure.Services.AuthService;
using Infrastructure.Services.ClassScheduleService;
using Infrastructure.Services.FileService;
using Infrastructure.Services.MembershipService;
using Infrastructure.Services.PaymentService;
using Infrastructure.Services.TrainerService;
using Infrastructure.Services.UserService;
using Infrastructure.Services.WorkoutService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();


var connection = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<DataContext>(conf => conf.UseNpgsql(connection));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
    {
        config.Password.RequiredLength = 8;
        config.Password.RequireDigit = true;
        config.Password.RequireNonAlphanumeric = true;
        config.Password.RequireUppercase = true;
        config.Password.RequireLowercase = true;
    })
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();



builder.Services.AddScoped<Seeder>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IClassScheduleService, ClassScheduleService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sample web API",
        Version = "version 1",
        Description = "Sample API Services.",
        Contact = new OpenApiContact
        {
            Name = "S Jons"
        },
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

try
{
    var serviceProvider = app.Services.CreateScope().ServiceProvider;
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();

    // seed data
    var seeder = serviceProvider.GetRequiredService<Seeder>();
    await seeder.SeedUser();
}
catch (Exception)
{
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//who are you? 
app.UseAuthentication();
//are you allowed? 
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();