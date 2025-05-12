using HrApp.Application.Department.DTO;
using MediatR;

namespace HrApp.Application.Department.Command.AddDepartment;

public class AddDepartmentCommand : DepartmentDTO, IRequest
{
    public string HeadOfDepartmentEmail { get; set; } = default!;
}