using LeaveRequests.Data;
using LeaveRequests.Migrations;
using LeaveRequests.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LeaveRequests.Services;

public class AuditService
{
    private readonly ApplicationDbContext _context;

    public AuditService (ApplicationDbContext context)
    {
        _context = context;        
    }

    public async Task Record(
        AuditAction action, 
        string actorUserId, 
        string actorEmail, 
        int leaveRequestId,
        LeaveStatus? oldStatus = null,
        LeaveStatus? newStatus = null,
        string? comment = null)
    {
        AuditEntry entry = new AuditEntry()
        {
            OccurredAt = DateTime.UtcNow,
            ActorUserId = actorUserId,
            ActorEmail = actorEmail,
            Action = action,
            LeaveRequestId = leaveRequestId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            Comment = comment
        };

        _context.AuditEntries.Add(entry);
    }
}