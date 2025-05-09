using HrApp.Application.Assignment.DTO;
using HrApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Assignment.Command.AddAssignment
{
    public class AddAssignmentCommand : AssignmentDTO, IRequest
    {
        public List<LeaderFeedback> LeaderFeedbacks { get; set; } = new List<LeaderFeedback>();
        public List<AssignmentNotification> AssignmentNotifications { get; set; } = new List<AssignmentNotification>();

    }
}
