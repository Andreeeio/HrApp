using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.WorkLog.Command.AddWorkLog
{
    public class AddWorkLogCommand : IRequest
    {
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
