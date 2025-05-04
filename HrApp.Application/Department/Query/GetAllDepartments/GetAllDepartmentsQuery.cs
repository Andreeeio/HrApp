using HrApp.Application.Department.DTO;
using MediatR;

namespace HrApp.Application.Department.Query.GetAllDepartments;

public class GetAllDepartmentsQuery : IRequest<List<DepartmentDTO>>
{
}