using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using WebClient.Handler;
using Microsoft.JSInterop;

namespace WebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDbContext<Context>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Default");
                if (connectionString != null)
                {
                    options.UseMySQL(connectionString);
                }
                else
                {
                    throw new Exception("Default connection string not found in configuration.");
                }
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 2;
            }).AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();

            builder.Services.AddHttpClient("HttpClient", (serviceProvider, httpClient) =>
            {
                IConfiguration? configuration = serviceProvider.GetRequiredService<IConfiguration>();
                string? apiHostName = configuration?["ApiHostName"];

                if (apiHostName != null)
                {
                    httpClient.BaseAddress = new Uri(apiHostName);
                }
                else
                {
                    throw new InvalidOperationException("ApiHostName is not configured.");
                }
            }).AddHttpMessageHandler<HttpClientAuthorizationHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            
            app.MapControllerRoute(name: "login",pattern: "{controller=Login}/{action=Login}/{id?}");
            app.MapControllers();
            app.MapRazorPages();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

                var superUser = userManager.FindByNameAsync("SuperUser").Result;
                if (superUser == null)
                {
                    var newSuperUser = new ApplicationUser
                    {
                        UserName = "SuperUser",
                        DisplayName = "SuperUser",
                        Email = "jess@jessicawylde.co.uk"
                    };
                    var result = userManager.CreateAsync(newSuperUser, "ChangeMe123!?").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(newSuperUser, "Admin").Wait();
                    }
                }
            }

            app.Run();
        }
    }
}