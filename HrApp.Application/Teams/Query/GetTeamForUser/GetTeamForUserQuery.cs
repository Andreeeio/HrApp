using HrApp.Application.Teams.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Teams.Query.GetTeamForUser
{
    public class GetTeamForUserQuery(Guid userId) : IRequest<TeamDTO>
    {
        public Guid UserId { get; set; } = userId;
    }
}
