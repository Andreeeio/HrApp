using HrApp.Application.WorkLog.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.WorkLog.Query.GetWorkLog
{
    public class GetWorkLogQuery(Guid id) : IRequest<List<WorkLogDTO>>
    {
        public Guid UserId { get; set; } = id;
    }
}
