using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Teams.Command.DeleteTeam
{
    public class DeleteTeamCommand : IRequest
    {
        public Guid TeamId { get; set; }
    }
}
