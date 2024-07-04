using DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using WebAPI.Helpers;
using Microsoft.AspNetCore.Http.Features;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<Context>(options =>
            {
                string? connectionString = builder.Configuration.GetConnectionString("Default");
                if (connectionString != null)
                {
                    options.UseMySQL(connectionString);
                }
                else
                {
                    throw new Exception("Default connection string not found in configuration.");
                }
            });

            builder.Services.AddScoped<ContentDataHelper>();
            builder.Services.AddCors(options =>
            {
                string? allowedOrigin = builder.Configuration["AllowedOrigin"];
                if (allowedOrigin != null)
                {
                    options.AddPolicy("AllowSpecificOrigin", builder =>
                    builder.WithOrigins(allowedOrigin).AllowAnyMethod().AllowAnyHeader());
                }
            });

            builder.Services.AddScoped<ContentDataHelper>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 2;
            }).AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<Context>();
                dbContext.Database.Migrate();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                context.Request.Scheme = "https";
                await next();
            });

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            //app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }

    }
}