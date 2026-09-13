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
public class AuditModel : PageModel
{
    public List<AuditEntry> Entries {get; set; } = new();
    private readonly ApplicationDbContext _context;
    public AuditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        Entries = await _context.AuditEntries
            .OrderByDescending(r => r.OccurredAt)
            .Take(200)
            .ToListAsync();
    }
}