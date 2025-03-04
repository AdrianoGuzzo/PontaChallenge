using System.Security.Claims;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            CreateUserDefault(userMgr, "adriano@email.com", "Adriano", "Guzzo");
            CreateUserDefault(userMgr, "admin@email.com", "Admin", "");           
        }

    }
    private static void CreateUserDefault(UserManager<ApplicationUser> userMgr, string userName, string givenName, string familyName)
    {
        var admin = userMgr.FindByNameAsync(userName).Result;
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(admin, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(admin, [
                            new(JwtClaimTypes.Name, $"{givenName} {familyName}"),
                            new(JwtClaimTypes.GivenName, givenName),
                            new(JwtClaimTypes.FamilyName, familyName),
                        ]).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug($"{givenName} created");
        }
        else
        {
            Log.Debug($"{givenName} already exists");
        }
    }
}
