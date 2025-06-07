using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Feedback.DTO
{
    public class AnonymousFeedbackDTO
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = default!;
        public string Message { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public Guid TeamId { get; set; }
    }
}
