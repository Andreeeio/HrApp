namespace HrApp.Application.EmployeeRates.Command.AddTaskRate;

public class AddTaskRateCommand
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public int Rate { get; set; }
}
