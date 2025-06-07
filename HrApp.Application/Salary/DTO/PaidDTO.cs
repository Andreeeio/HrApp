using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Salary.DTO
{
    public class PaidDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public float BaseSalary { get; set; }
    }
}
