using HrApp.Application.Teams.DTO;
using MediatR;

namespace HrApp.Application.Teams.Query.GetTeamForDepartment;

public class GetTeamForDepartmentQuery(Guid deptId) : IRequest<List<TeamDTO>>
{
    public Guid DepartmentId { get; set; } = deptId;
}

