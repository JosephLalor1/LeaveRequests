/**/

using LeaveRequests.Data;
using LeaveRequests.Models;
using LeaveRequests.Services;
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
    private readonly AuditService _auditService;
    public ApprovalsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, AuditService auditService)
    {
        _context = context;
        _userManager = userManager;
        _auditService = auditService;
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

        var userId = _userManager.GetUserId(User);
        string email = User.Identity?.Name ?? "";

        if (request == null)
        {
            return NotFound();
        }

        request.Status = LeaveStatus.Approved;

        _auditService.Record(AuditAction.Approved, userId, email, request.Id, LeaveStatus.Pending, LeaveStatus.Approved);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRejectAsync(int id)
    {
        var request = await _context.LeaveRequests.FindAsync(id);

        var userId = _userManager.GetUserId(User);
        string email = User.Identity?.Name ?? "";

        if (request == null)
        {
            return NotFound();
        }

        request.Status = LeaveStatus.Rejected;

        _auditService.Record(AuditAction.Rejected, userId, email, request.Id, LeaveStatus.Pending, LeaveStatus.Rejected);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}