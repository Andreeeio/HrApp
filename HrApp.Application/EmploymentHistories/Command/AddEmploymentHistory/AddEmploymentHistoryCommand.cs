using MediatR;

namespace HrApp.Application.EmploymentHistories.Command.AddEmploymentHistory;

public class AddEmploymentHistoryCommand : IRequest
{
    public string Position { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Email { get; set; } = default!;
}
