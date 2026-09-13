/*Created: 08/09/2026
By: Joseph Lalor
Project: Leave Requests
Description: Enum for the actions taken on audit log*/

namespace LeaveRequests.Models;

public enum AuditAction
{
    Submitted,
    Approved,
    Rejected,
    Cancelled
}