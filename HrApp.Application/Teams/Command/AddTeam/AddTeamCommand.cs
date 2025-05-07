using HrApp.Application.Teams.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Teams.Command.AddTeam
{
    public class AddTeamCommand : TeamDTO, IRequest
    {
        public string? TeamLeaderEmail { get; set; }
    }
}
