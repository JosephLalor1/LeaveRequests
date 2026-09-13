/*Created: 09/09/2026
By: Joseph Lalor
Project: Leave Requests
Description: Creates Audit Entry object and database table*/

namespace LeaveRequests.Models;

public class AuditEntry
{
    public int Id {get; set; }
    public DateTime OccurredAt {get; set; }
    public string ActorUserId {get; set; } = string.Empty;
    public string ActorEmail {get; set; } = string.Empty;
    public AuditAction Action {get; set; }
    public int LeaveRequestId {get; set; }
    public LeaveStatus? OldStatus {get; set; }
    public LeaveStatus? NewStatus {get; set; }
    public string? Comment {get; set; }
}