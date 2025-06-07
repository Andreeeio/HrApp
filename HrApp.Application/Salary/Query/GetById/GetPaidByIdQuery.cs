using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Salary.Query.GetById
{
    public class GetPaidByIdQuery(Guid id) : IRequest<Domain.Entities.Paid>
    {
        public Guid Id { get; set; } = id;
    }
}
