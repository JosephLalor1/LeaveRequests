/*Created: 08/09/2026
By: Joseph Lalor
Project: Leave Requests
Description: Sets up roles and seeds one manager user*/

using Microsoft.AspNetCore.Identity;

namespace LeaveRequests.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
    
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityRole>>();

        string[] roles = {"Manager", "Staff"};

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    
    }
    
}

