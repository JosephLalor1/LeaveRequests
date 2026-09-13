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
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roles = {"Manager", "Staff"};

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var manager = await userManager.FindByEmailAsync("manager@example.com");

        if (manager == null)
        {
            manager = new IdentityUser {UserName = "manager@example.com", Email = "manager@example.com", EmailConfirmed = true};
            await userManager.CreateAsync(manager, "Test1234!");
            await userManager.AddToRoleAsync(manager, "Manager");
        }

        var staff = await userManager.FindByEmailAsync("staff@example.com");

        if (staff == null)
        {
            staff = new IdentityUser {UserName = "staff@example.com", Email = "staff@example.com", EmailConfirmed = true};
            await userManager.CreateAsync(staff, "Test1234!");
            await userManager.AddToRoleAsync(staff, "Staff");
        }
    
    }
    
}

