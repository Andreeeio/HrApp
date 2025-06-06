using HrApp.Application.Salary.DTO;
using MediatR;

namespace HrApp.Application.Salary.Command.UpdatePaid;

public class UpdatePaidCommand : PaidDTO, IRequest
{
    public string Email { get; set; } = default!;
}
