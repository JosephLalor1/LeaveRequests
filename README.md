# Leave Request System
 
A staff annual leave system with role-based approval and an append-only audit trail. Built with ASP.NET Core Razor Pages, Entity Framework Core and ASP.NET Core Identity.
 
Staff submit leave requests. Managers approve or reject them. Every change is recorded in a log that can't be edited or deleted.
 
<img width="949" height="499" alt="Screenshot 2026-09-13 230834" src="https://github.com/user-attachments/assets/86f1d87c-7a65-4bb1-8dfe-ca6873f7ec08" />

 
## Try it in thirty seconds
 
```bash
git clone https://github.com/JosephLalor01/LeaveRequests.git
cd LeaveRequests
dotnet tool install --global dotnet-ef
dotnet ef database update
dotnet run
```
 
Open the URL printed in the terminal and sign in with either demo account. The database is created and seeded on first run, so there's data to look at immediately.
 
| Role    | Email                 | Password    | What you can do                                   |
|---------|-----------------------|-------------|---------------------------------------------------|
| Staff   | `staff@example.com`   | `Test1234!` | Submit requests, view your own history            |
| Manager | `manager@example.com` | `Test1234!` | Approve or reject pending requests, view the audit log |
 
Requires the [.NET SDK](https://dotnet.microsoft.com/download). No database server to install — SQLite writes a single file in the project folder.
 
## What it does
 
**Staff**
- Submit a leave request with dates, type (annual, sick, unpaid) and a reason
- See every request they've made and its current status
**Managers**
- See a queue of pending requests from other staff
- Approve with one click, or reject with a mandatory reason
- Browse the full audit log
**Every state change is logged.** Submission, approval and rejection each write an entry recording who acted, when, which request, the status before and after, and any comment. The audit page is read-only by design: no edit, no delete, no buttons.

Every state change is logged. Submission, approval and rejection each write an entry recording who acted, when, which request, the status before and after, and any comment. The audit page is read-only by design: no edit, no delete, no buttons.
