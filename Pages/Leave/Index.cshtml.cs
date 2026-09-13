/*Created: 08/09/2026
By: Joseph Lalor
Project: Leave Requests
Description: Dashboard for viewing requests*/

using LeaveRequests.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LeaveRequests.Models;

namespace LeaveRequests.Pages.Leave;

[Authorize]

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _connection;
    private readonly UserManager<IdentityUser> _userManager;
    public List<LeaveRequest> Requests {get; set; } = new();
    public IndexModel (ApplicationDbContext connection, UserManager<IdentityUser> userManager)
    {
        _connection = connection;
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);

        Requests = await  _connection.LeaveRequests
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.SubmittedAt)
            .ToListAsync();
    }
}