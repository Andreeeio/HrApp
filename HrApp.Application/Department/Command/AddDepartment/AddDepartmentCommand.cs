using HrApp.Application.Department.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Department.Command.AddDepartment
{
    public class AddDepartmentCommand : DepartmentDTO, IRequest
    {
        public string HeadOfDepartmentEmail { get; set; } = default!;
    }
}
