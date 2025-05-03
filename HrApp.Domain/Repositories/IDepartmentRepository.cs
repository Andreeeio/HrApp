using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Domain.Repositories
{
    public interface IDepartmentRepository
    {
        public Task<List<Department>> GetAllDepartments();
    }
}
