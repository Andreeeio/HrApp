using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using HrApp.Infrastructure.Presistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Infrastructure.Repositories
{
    public class SalaryRepository : ISalaryRepository
    {
        private readonly HrAppContext _dbcontext;
        public SalaryRepository(HrAppContext context)
        {
            _dbcontext = context;
        }
        public Task AddEmployeeRate(EmployeeRate employeeRate)
        {
            _dbcontext.EmployeeRate.AddAsync(employeeRate);
            return _dbcontext.SaveChangesAsync();

        }

        public Task AddPaid(Paid paid)
        {
            _dbcontext.Paid.AddAsync(paid);
            return _dbcontext.SaveChangesAsync();
        }

        public Task<Paid> GetPaidByUserId(Guid userid)
        {
            return Task.FromResult(_dbcontext.Paid.FirstOrDefault(x => x.UserId == userid));
        }
        public Task<Paid> GetPaidById(Guid paidid)
        {
            return Task.FromResult(_dbcontext.Paid.FirstOrDefault(x => x.Id == paidid));
        }
        public Task UpdatePaid(Paid paid)
        {
            _dbcontext.Paid.Update(paid);
            return _dbcontext.SaveChangesAsync();
        }
    }
}
