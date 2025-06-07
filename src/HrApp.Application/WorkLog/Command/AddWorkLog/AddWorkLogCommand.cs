using MediatR;

namespace HrApp.Application.WorkLog.Command.AddWorkLog;

public class AddWorkLogCommand : IRequest
{
    public Guid UserId { get; set; }
    public DateTime StartTime { get; set; }
}