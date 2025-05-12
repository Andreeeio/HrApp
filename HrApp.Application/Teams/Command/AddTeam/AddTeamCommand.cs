using HrApp.Application.Teams.DTO;
using MediatR;

namespace HrApp.Application.Teams.Command.AddTeam;

public class AddTeamCommand : TeamDTO, IRequest
{
    public string? TeamLeaderEmail { get; set; }
}