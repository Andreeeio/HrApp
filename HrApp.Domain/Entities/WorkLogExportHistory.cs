using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Domain.Entities
{
    public class WorkLogExportHistory
    {
        public Guid Id { get; set; }
        public Guid ExportedByUserId { get; set; }
        public virtual User ExportedByUser { get; set; } = default!;
        public Guid ExportedForUserId { get; set; }
        public virtual User ExportedForUser { get; set; } = default!;
        public DateTime ExportDate { get; set; }       
    }
}
