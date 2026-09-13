/*Created: 08/09/2026
By: Joseph Lalor
Project: Leave Requests
Description: Enum for the status of a leave request*/

namespace LeaveRequests.Models;

public enum LeaveStatus
{
    Pending,
    Approved,
    Rejected,
    Cancelled
}