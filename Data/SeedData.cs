/*Created: 08/09/2026
By: Joseph Lalor
Project: Leave Requests
Description: Sets up roles and seeds manager user and staff and seeds some entries*/

using LeaveRequests.Models;
using Microsoft.AspNetCore.Identity;

namespace LeaveRequests.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        string[] roles = { "Manager", "Staff" };

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
            manager = new IdentityUser { UserName = "manager@example.com", Email = "manager@example.com", EmailConfirmed = true };
            var result = await userManager.CreateAsync(manager, "Test1234!");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Manager seed failed: {error.Description}");
                }
                return;
            }

            await userManager.AddToRoleAsync(manager, "Manager");
        }

        var staff = await userManager.FindByEmailAsync("staff@example.com");

        if (staff == null)
        {
            staff = new IdentityUser { UserName = "staff@example.com", Email = "staff@example.com", EmailConfirmed = true };
            var result = await userManager.CreateAsync(staff, "Test1234!");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Staff seed failed: {error.Description}");
                }
                return;
            }

            await userManager.AddToRoleAsync(staff, "Staff");
        }

        if (!context.LeaveRequests.Any())
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var approved = new LeaveRequest
            {
                UserId = staff.Id,
                StartDate = today.AddDays(-30),
                EndDate = today.AddDays(-26),
                Type = LeaveType.Annual,
                Reason = "Family holiday",
                Status = LeaveStatus.Approved,
                SubmittedAt = DateTime.UtcNow.AddDays(-45)
            };

            var rejected = new LeaveRequest
            {
                UserId = staff.Id,
                StartDate = today.AddDays(-10),
                EndDate = today.AddDays(-9),
                Type = LeaveType.Unpaid,
                Reason = "Personal matter",
                Status = LeaveStatus.Rejected,
                SubmittedAt = DateTime.UtcNow.AddDays(-20)
            };

            var pending = new LeaveRequest
            {
                UserId = staff.Id,
                StartDate = today.AddDays(21),
                EndDate = today.AddDays(25),
                Type = LeaveType.Annual,
                Reason = "Wedding",
                Status = LeaveStatus.Pending,
                SubmittedAt = DateTime.UtcNow.AddDays(-2)
            };

            context.LeaveRequests.AddRange(approved, rejected, pending);
            await context.SaveChangesAsync();

            context.AuditEntries.AddRange(
                new AuditEntry
                {
                    OccurredAt = approved.SubmittedAt,
                    ActorUserId = staff.Id,
                    ActorEmail = staff.Email!,
                    Action = AuditAction.Submitted,
                    LeaveRequestId = approved.Id,
                    NewStatus = LeaveStatus.Pending
                },
                new AuditEntry
                {
                    OccurredAt = approved.SubmittedAt.AddDays(1),
                    ActorUserId = manager.Id,
                    ActorEmail = manager.Email!,
                    Action = AuditAction.Approved,
                    LeaveRequestId = approved.Id,
                    OldStatus = LeaveStatus.Pending,
                    NewStatus = LeaveStatus.Approved
                },
                new AuditEntry
                {
                    OccurredAt = rejected.SubmittedAt,
                    ActorUserId = staff.Id,
                    ActorEmail = staff.Email!,
                    Action = AuditAction.Submitted,
                    LeaveRequestId = rejected.Id,
                    NewStatus = LeaveStatus.Pending
                },
                new AuditEntry
                {
                    OccurredAt = rejected.SubmittedAt.AddDays(1),
                    ActorUserId = manager.Id,
                    ActorEmail = manager.Email!,
                    Action = AuditAction.Rejected,
                    LeaveRequestId = rejected.Id,
                    OldStatus = LeaveStatus.Pending,
                    NewStatus = LeaveStatus.Rejected,
                    Comment = "Not enough cover that week"
                },
                new AuditEntry
                {
                    OccurredAt = pending.SubmittedAt,
                    ActorUserId = staff.Id,
                    ActorEmail = staff.Email!,
                    Action = AuditAction.Submitted,
                    LeaveRequestId = pending.Id,
                    NewStatus = LeaveStatus.Pending
                });

            await context.SaveChangesAsync();
        }
    }
}