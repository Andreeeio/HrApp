using HrApp.Application.Department.Query.GetAllDepartments;
using HrApp.Application.Teams.Command.AddEmployer;
using HrApp.Application.Teams.Command.AddTeam;
using HrApp.Application.Teams.Query.GetEmployersInTeam;
using HrApp.Application.Teams.Query.GetTeamForDepartment;
using HrApp.MVC.Views.Team;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HrApp.Application.Teams.Query.GetAllTeams;
using HrApp.Domain.Entities;
using HrApp.Application.Users.Query.GetDataFromToken;
using HrApp.Application.Teams.Query.GetTeamForUser;
using HrApp.Application.Users.Query.GetUserByEmail;
using HrApp.Application.Teams.Command.DeleteUserFromTeam;
using HrApp.Application.Teams.Command.DeleteTeam;
using HrApp.Application.Feedback.Command;
using HrApp.Application.Feedback.Query;

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
        ViewBag.IdDept = id;
        return View(teams);
    }

    [HttpGet("employers/{TeamId}/{TeamName}")]
    public async Task<IActionResult> EmployersInTeam(Guid Teamid, string TeamName)
    {
        var employers = await _sender.Send(new GetEmployersInTeamQuery(Teamid));
        ViewBag.TeamId = Teamid;
        ViewBag.TeamName = TeamName;
        return View(employers);
    }

    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var user = await _sender.Send(new GetDataFromTokenQuery());
        var team = await _sender.Send(new GetTeamForUserQuery(Guid.Parse(user.id)));

        if (team == null)
        {
            return View("NoTeam");
        }

        // Przekierowanie do akcji EmployersInTeam z parametrami TeamId i TeamName
        return RedirectToAction("EmployersInTeam", new { TeamId = team.Id, TeamName = team.Name });
    }

    [HttpGet("{id}/Create")]
    public async Task<IActionResult> Create(Guid id)
    {
        var departments = await _sender.Send(new GetAllDepartmentsQuery());
        ViewBag.Departments = new SelectList(departments, "Id", "Name");
        ViewBag.DeptID = id;
        return View();
    }

    [HttpPost("{id}/Create")]
    public async Task<IActionResult> Create(AddTeamCommand command)
    {
        if (!ModelState.IsValid)
        {
            // Handle the command to create a team
            return View(command);
        }
        var user = await _sender.Send(new GetUserByEmailQuery(command.TeamLeaderEmail));
        command.TeamLeaderId = user.Id;
        await _sender.Send(command);
        return RedirectToAction("Index","Departments");
    }

    [HttpGet("AddEmployer")]
    public async Task<IActionResult> AddEmployer()
    {
        var teams = await _sender.Send(new GetAllTeamsQuery());
        ViewBag.teams = new SelectList(teams, "Id", "Name");
        return View();
    }

    [HttpPost("AddEmployer")]
    public async Task<IActionResult> AddEmployer(AddEmployerCommand command)
    {
        if (!ModelState.IsValid)
        {
            // Handle the command to create a team
            return View(command);
        }
        var user = await _sender.Send(new GetUserByEmailQuery(command.UserEmail));
        command.UserId = user.Id;
        await _sender.Send(command);
        return RedirectToAction("Index", "Departments");
    }

    [HttpGet("DeleteUserFromTeam/{UserId}/{TeamId}")]
    public async Task<IActionResult> DeleteUserFromTeam(Guid UserId, Guid TeamId)
    {
        var command = new DeleteUserFromTeamCommand
        {
            UserId = UserId,
            TeamId = TeamId
        };
        await _sender.Send(command);
        return RedirectToAction("Index", "Departments");
    }

    [HttpGet("DeleteTeam/{id}")]
    public async Task<IActionResult> DeleteTeam(Guid id)
    {
        var command = new DeleteTeamCommand
        {
            TeamId = id
        };
        await _sender.Send(command);
        return RedirectToAction("Index", "Departments");
    }

    [HttpGet("LeaveAnonymousFeedback/{teamId}")]
    public IActionResult LeaveAnonymousFeedback(Guid teamId)
    {
        var model = new AddAnonymousFeedbackCommand { TeamId = teamId };
        return View(model);
    }

    [HttpPost("LeaveAnonymousFeedback/{teamId}")]
    public async Task<IActionResult> LeaveAnonymousFeedback(AddAnonymousFeedbackCommand command)
    {
        await _sender.Send(command);
        TempData["SuccessMessage"] = "Your feedback has been submitted anonymously.";
        return RedirectToAction("Index");
    }
    [HttpGet("Feedbacks/{teamId}")]
    public async Task<IActionResult> ViewAnonymousFeedbacks(Guid teamId)
    {
        var feedbacks = await _sender.Send(new GetAnonymousFeedbacksForTeamQuery(teamId));
        ViewBag.TeamId = teamId;
        return View(feedbacks);
    }
}