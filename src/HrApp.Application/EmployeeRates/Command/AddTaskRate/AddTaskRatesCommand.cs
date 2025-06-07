using MediatR;

namespace HrApp.Application.EmployeeRates.Command.AddTaskRate;

public class AddTaskRatesCommand : IRequest
{
    public List<AddTaskRateCommand> TaskRates { get; set; } = default!;
}
