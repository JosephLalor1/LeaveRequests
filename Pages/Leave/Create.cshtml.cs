using LeaveRequests.Data;
using LeaveRequests.Models;
using LeaveRequests.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LeaveRequests.Pages.Leave;

[Authorize]
public class CreateModel : PageModel
{
    [BindProperty]
    public LeaveRequest Input {get; set; } = new();
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AuditService _auditService;
    public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, AuditService auditService)
    {
        _context = context;
        _userManager = userManager;
        _auditService = auditService;
    }

    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var userId = _userManager.GetUserId(User);
        Input.UserId = userId;
        Input.Status = LeaveStatus.Pending;
        Input.SubmittedAt = DateTime.UtcNow;
        string email = User.Identity?.Name ?? "";
        
        _context.LeaveRequests.Add(Input);
        await _context.SaveChangesAsync();

        _auditService.Record(AuditAction.Submitted, userId, email, Input.Id);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}