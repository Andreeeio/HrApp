using HrApp.Application.Salary.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Salary.Command.AddPaid
{
    public class AddPaidCommand : PaidDTO, IRequest
    {
        public string Email { get; set; } = default!;
    }
}
