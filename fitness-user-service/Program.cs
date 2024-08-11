using fitness_db.Data;
using fitness_db.Interfaces;
using fitness_db.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace fitness_user_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            bool useCors = builder.Configuration.GetValue<bool>("CorsSettings:UseCors");
            if (useCors)
            {
                string[] allowedOrigins = useCors
                    ? builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>()
                    : Array.Empty<string>();

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowConfiguredOrigins", policy =>
                    {
                        policy.WithOrigins(allowedOrigins)
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
                });
            }


            builder.Services.AddDbContext<FitnessContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserRepository, UserRepository>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "sapoetrandi",
                    ValidAudience = "user-service",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"))
                };
            });

            var app = builder.Build();

            if (useCors)
            {
                app.UseCors("AllowConfiguredOrigins");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
