using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HrApp.Domain.Entities;

namespace HrApp.Application.Department.DTO
{
    public class DepartmentDTO
    {
        public string Name { get; set; } = default!;
        public List<HrApp.Domain.Entities.Team> Teams { get; set; } = default!;
        public Guid HeadOfDepartmentId { get; set; }
        public string TeamTag { get; set; } = default!;

    }
}
