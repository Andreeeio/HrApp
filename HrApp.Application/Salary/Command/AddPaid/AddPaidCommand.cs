using HrApp.Application.Salary.DTO;
using MediatR;

namespace HrApp.Application.Salary.Command.AddPaid;

public class AddPaidCommand : PaidDTO, IRequest
{
    public string Email { get; set; } = default!;
}
