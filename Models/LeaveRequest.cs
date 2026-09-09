/*Created: 09/09/2026
By: Joseph Lalor
Project: Leave Requests
Description: Creates Leave Request object and database table*/

namespace LeaveRequests.Models;

public class LeaveRequest
{
    public int Id {get; set; }
    public string UserId {get; set; } = string.Empty;
    public DateOnly StartDate {get; set; }
    public DateOnly EndDate {get; set; }
    public string Reason {get; set; } = string.Empty;
    public DateTime SubmittedAt {get; set; }
    public LeaveStatus Status {get; set; }
    public LeaveType Type {get; set; }
}