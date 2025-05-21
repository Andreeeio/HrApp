using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Domain.Repositories
{
    public interface ISalaryRepository
    {
        Task AddEmployeeRate(EmployeeRate employeeRate);
        Task AddPaid(Paid paid);
        Task<Paid> GetPaidByUserId(Guid userid);
        Task<Paid> GetPaidById(Guid paidid);
        Task UpdatePaid(Paid paid);
    }
}
