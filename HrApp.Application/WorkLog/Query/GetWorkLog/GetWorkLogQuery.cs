using HrApp.Application.WorkLog.DTO;
using MediatR;

namespace HrApp.Application.WorkLog.Query.GetWorkLog;

public class GetWorkLogQuery(Guid id) : IRequest<List<WorkLogDTO>>
{
    public Guid UserId { get; set; } = id;
}