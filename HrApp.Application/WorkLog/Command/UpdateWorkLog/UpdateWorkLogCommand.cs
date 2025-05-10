using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.WorkLog.Command.UpdateWorkLog
{
    public class UpdateWorkLogCommand : IRequest
    {
        public Guid Id { get; set; }
        public DateTime? EndTime { get; set; }
        public int Hours { get; set; }
    }
}
