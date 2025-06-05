using HrApp.Application.Department.Query.GetAllDepartments;
using HrApp.Application.Teams.Command.AddEmployer;
using HrApp.Application.Teams.Command.AddTeam;
using HrApp.Application.Teams.Query.GetEmployersInTeam;
using HrApp.Application.Teams.Query.GetTeamForDepartment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HrApp.Application.Teams.Query.GetAllTeams;
using HrApp.Application.Teams.Query.GetTeamForUser;
using HrApp.Application.Teams.Command.DeleteUserFromTeam;
using HrApp.Application.Teams.Command.DeleteTeam;
using HrApp.Application.Feedback.Command;
using HrApp.Application.Feedback.Query;
using Microsoft.AspNetCore.Authorization;
using HrApp.Domain.Exceptions;

namespace HrApp.MVC.Controllers;

[Authorize]
[Route("Team")]
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

    [HttpGet("Employers/{TeamId}/{TeamName}")]
    public async Task<IActionResult> EmployersInTeam(Guid Teamid, string TeamName)
    {
        var employers = await _sender.Send(new GetEmployersInTeamQuery(Teamid));
        ViewBag.TeamId = Teamid;
        ViewBag.TeamName = TeamName;
        return View(employers);
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var team = await _sender.Send(new GetTeamForUserQuery());

            return RedirectToAction("EmployersInTeam", new { TeamId = team.Id, TeamName = team.Name });
        }
        catch (BadRequestException)
        {
            TempData["ErrorMessage"] = "You are not assigned to any team.";
            return RedirectToAction("Index", "Departments");
        }
    }

    [Authorize(Roles = "Ceo, Hr")]
    [HttpGet("{id}/Create")]
    public async Task<IActionResult> Create(Guid id)
    {
        var departments = await _sender.Send(new GetAllDepartmentsQuery());
        ViewBag.Departments = new SelectList(departments, "Id", "Name");
        ViewBag.DeptID = id;
        return View();
    }

    [Authorize(Roles = "Ceo, Hr")]
    [HttpPost("{id}/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid id, AddTeamCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }
        try
        {
            await _sender.Send(command);
        }
        catch (BadRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("TeamInDept", new { id });
        }
        return RedirectToAction("TeamInDept", new { id });
    }

    [Authorize(Roles = "Ceo ,Hr, TeamLeader")]
    [HttpGet("AddEmployer")]
    public async Task<IActionResult> AddEmployer()
    {
        var teams = await _sender.Send(new GetAllTeamsQuery());
        ViewBag.teams = new SelectList(teams, "Id", "Name");
        return View();
    }

    [Authorize(Roles = "Ceo ,Hr, TeamLeader")]
    [HttpPost("AddEmployer")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddEmployer(AddEmployerCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }
        var userId = await _sender.Send(command);
        try
        {
            var team = await _sender.Send(new GetTeamForUserQuery(userId));
            return RedirectToAction("EmployersInTeam", new { TeamId = command.TeamId, TeamName = team.Name });
        }
        catch (BadRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index", "Departments");
        }
    }

    [Authorize(Roles = "Ceo ,Hr, TeamLeader")]
    [HttpPost("DeleteUserFromTeam/{UserId}/{TeamId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUserFromTeam(Guid UserId, Guid TeamId)
    {
        var command = new DeleteUserFromTeamCommand
        {
            UserId = UserId,
            TeamId = TeamId
        };

        try
        {
            var team = await _sender.Send(new GetTeamForUserQuery(UserId));
            await _sender.Send(command);
            TempData["SuccessMessage"] = "User has been removed from the team successfully.";
            return RedirectToAction("EmployersInTeam", new { TeamId = command.TeamId, TeamName = team.Name });
        }
        catch (BadRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index", "Departments");
        }
    }

    [Authorize(Roles = "Ceo ,Hr, TeamLeader")]
    [HttpPost("DeleteTeam/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTeam(Guid id)
    {
        var command = new DeleteTeamCommand
        {
            TeamId = id
        };
        try
        {
            TempData["SuccessMessage"] = "Team has been deleted successfully.";
            await _sender.Send(command);
        }
        catch (BadRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index", "Departments");
        }
        return RedirectToAction("Index", "Departments");
    }

    [HttpGet("LeaveAnonymousFeedback/{teamId}")]
    public IActionResult LeaveAnonymousFeedback(Guid teamId)
    {
        var model = new AddAnonymousFeedbackCommand { TeamId = teamId };
        return View(model);
    }

    [HttpPost("LeaveAnonymousFeedback/{teamId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LeaveAnonymousFeedback(Guid teamId, AddAnonymousFeedbackCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }

        await _sender.Send(command);
        TempData["SuccessMessage"] = "Your feedback has been submitted anonymously.";
        return RedirectToAction("ViewAnonymousFeedbacks", new { teamId});
    }

    [HttpGet("Feedbacks/{teamId}")]
    public async Task<IActionResult> ViewAnonymousFeedbacks(Guid teamId)
    {
        var feedbacks = await _sender.Send(new GetAnonymousFeedbacksForTeamQuery(teamId));
        ViewBag.TeamId = teamId;
        return View(feedbacks);
    }
}