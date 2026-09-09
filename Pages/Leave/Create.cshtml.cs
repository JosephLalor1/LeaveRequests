using LeaveRequests.Data;
using LeaveRequests.Models;
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
    public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        context = _context;
        userManager = _userManager;
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
        
        _context.LeaveRequests.Add(Input);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}