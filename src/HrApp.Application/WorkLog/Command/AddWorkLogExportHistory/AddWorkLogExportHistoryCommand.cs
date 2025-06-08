using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.WorkLog.Command.AddWorkLogExportHistory
{
    public class AddWorkLogExportHistoryCommand : IRequest
    {
        public Guid ExportedByUserId { get; set; }
        public Guid ExportedForUserId { get; set; }
        public DateTime ExportDate { get; set; }
    }
}
