using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Department.Command.DeleteDepartment
{
    public class DeleteDepartmentCommand : IRequest
    {
        public Guid DepartmentId { get; set; }
    }
}
