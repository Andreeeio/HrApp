using HrApp.Application.Department.DTO;
using HrApp.Application.Team.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Department.Query.GetAllDepartments
{
    public class GetAllDepartmentsQuery : IRequest<List<DepartmentDTO>>
    {
    }
}
