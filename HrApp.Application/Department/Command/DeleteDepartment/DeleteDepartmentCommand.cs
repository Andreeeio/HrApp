using MediatR;

namespace HrApp.Application.Department.Command.DeleteDepartment;

public class DeleteDepartmentCommand : IRequest
{
    public Guid DepartmentId { get; set; }
}