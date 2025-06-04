using FluentValidation;

namespace HrApp.Application.EmployeeRates.Command.AddTaskRate;

public class AddTaskRatesCommandValidator : AbstractValidator<AddTaskRatesCommand>
{
    public AddTaskRatesCommandValidator()
    {
        RuleFor(x => x.TaskRates)
            .NotEmpty().WithMessage("Task rates cannot be empty.")
            .Must(rates => rates.Count >= 1 && rates.Count <= 5)
            .WithMessage("You must provide between 1 and 5 task rates.");
    }
}
