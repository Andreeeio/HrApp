using HrApp.Application.Users.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Teams.Command.DeleteUserFromTeam
{
    public class DeleteUserFromTeamCommand : IRequest
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
    }
}
