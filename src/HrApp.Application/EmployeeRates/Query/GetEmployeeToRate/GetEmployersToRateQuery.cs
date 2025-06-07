using HrApp.Application.EmployeeRates.Command.AddTaskRate;
using MediatR;

namespace HrApp.Application.EmployeeRates.Query.GetEmployeeToRate;

public class GetEmployersToRateQuery : IRequest<AddTaskRatesCommand>
{
    public Guid TeamId { get; set; }
    public GetEmployersToRateQuery(Guid teamId)
    {
        TeamId = teamId;
    }
}
