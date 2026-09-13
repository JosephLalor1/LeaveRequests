/**/

using LeaveRequests.Data;
using LeaveRequests.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LeaveRequests.Pages.Leave;

[Authorize(Roles = "Manager")]
public class ApprovalsModel : PageModel
{
    public List<LeaveRequest> Requests {get; set; } = new();
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    public ApprovalsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);

        Requests = await _context.LeaveRequests
            .Where(r => r.UserId != userId && r.Status == LeaveStatus.Pending)
            .OrderByDescending(r => r.SubmittedAt)
            .ToListAsync();
    }
    
    public async Task<IActionResult> OnPostApproveAsync(int id)
    {
        var request = await _context.LeaveRequests.FindAsync(id);

        if (request == null)
        {
            return NotFound();
        }

        request.Status = LeaveStatus.Approved;

        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRejectAsync(int id)
    {
        var request = await _context.LeaveRequests.FindAsync(id);

        if (request == null)
        {
            return NotFound();
        }

        request.Status = LeaveStatus.Rejected;

        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}