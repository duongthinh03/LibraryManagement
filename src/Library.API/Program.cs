using Library.Application.Common.Authorization;
using Library.Application.Interfaces;
using Library.Application.Services;
using Library.Domain.Interfaces;
using Library.Infrastructure;
using Library.Infrastructure.Persistence;
using Library.Infrastructure.Persistence.Repositories;
using Library.Infrastructure.Sevices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Library.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                    ),
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.BookCreate, policy =>
                    policy.RequireClaim("permission", Permissions.BookCreate));

                options.AddPolicy(Permissions.BookDelete, policy =>
                    policy.RequireClaim("permission", Permissions.BookDelete));

                options.AddPolicy(Permissions.BookView, policy =>
                    policy.RequireClaim("permission", Permissions.BookView));
            });

            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IReaderRepository, ReaderRepository>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
