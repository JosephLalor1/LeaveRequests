/*Created: 08/09/2026
By: Joseph Lalor
Project: Leave Requests
Description: Provides a service which can be used for adding a new entry in the audit database*/

using LeaveRequests.Data;
using LeaveRequests.Models;

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