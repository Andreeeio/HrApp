using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Salary.Query.GetByUserId
{
    public class GetPaidByUserIdQuery : IRequest<Domain.Entities.Paid>
    {
        public Guid Id { get; }

        public GetPaidByUserIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
