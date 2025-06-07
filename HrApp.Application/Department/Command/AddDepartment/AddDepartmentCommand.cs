using HrApp.Application.Department.DTO;
using HrApp.Domain.Entities;
using MediatR;

namespace HrApp.Application.Department.Command.AddDepartment;

public class AddDepartmentCommand : IRequest
{
    public string Name { get; set; } = default!;
    public string TeamTag { get; set; } = default!;
    public string HeadOfDepartmentEmail { get; set; } = default!;
}