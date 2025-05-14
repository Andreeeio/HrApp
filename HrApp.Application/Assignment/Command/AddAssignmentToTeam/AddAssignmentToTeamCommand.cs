using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Assignment.Command.AddAssignmentToTeam
{
    public class AddAssignmentToTeamCommand : IRequest
    {
        public Guid AssignmentId { get; set; }
        public Guid TeamId { get; set; }
    }
}
