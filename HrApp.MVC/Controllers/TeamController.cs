using HrApp.Application.Teams.Query.GetEmployersInTeam;
using HrApp.Application.Teams.Query.GetTeamForDepartment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HrApp.MVC.Controllers;

[Route("team")]
public class TeamController : Controller
{
    private readonly ISender _sender;
    public TeamController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> TeamInDept(Guid id)
    {
        var teams = await _sender.Send(new GetTeamForDepartmentQuery(id));
        return View(teams);
    }

    [HttpGet("{id}/employers")]
    public async Task<IActionResult> EmployersInTeam(Guid id)
    {
        var employers = await _sender.Send(new GetEmployersInTeamQuery(id));
        return View(employers);
    }
}
