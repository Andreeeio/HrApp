using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediatR;
using HrApp.Application.Assignment.Query.GetAssignmentForTeam;
using HrApp.Application.Teams.Query.GetAllTeams;
using HrApp.Application.Assignment.Command.AddAssignment;
using HrApp.Application.Teams.Query.GetTeamForUser;
using HrApp.Application.Users.Query.GetDataFromToken;
using HrApp.Application.Assignment.Query.GetActiveAssignments;
using HrApp.Application.Assignment.Query.GetFreeAssignments;
using HrApp.Application.Assignment.Command.AddAssignmentToTeam;
using HrApp.Application.Assignment.Query.GetAssignmentById;
using HrApp.Application.Assignment.Command.EditAssignment;
using HrApp.Domain.Exceptions;
using HrApp.Application.Assignment.Command.CompleteAssignment;
using Microsoft.AspNetCore.Authorization;
using HrApp.Application.Assignment.DTO;

namespace HrApp.MVC.Controllers;

[Authorize(Roles = "Junior, Mid, Senior, TeamLeader, Hr, Ceo")]
[Route("Assignment")]
public class AssignmentController : Controller
{
    public readonly ISender _sender;

    public AssignmentController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{TeamId}/Assignments")]
    public async Task<IActionResult> Index(Guid TeamId)
    {
        var assignments = await _sender.Send(new GetAssignmentForTeamQuery(TeamId));
        ViewBag.TeamId = TeamId;
        ViewBag.Title = "Assignments for Team";
        return View(assignments);
    }

    [Authorize(Roles = "Hr, Ceo")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var teams = await _sender.Send(new GetAllTeamsQuery());
        ViewBag.teams = new SelectList(teams, "Id", "Name");
        var teamList = teams.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name
        }).ToList();

        teamList.Insert(0, new SelectListItem
        {
            Value = "",
            Text = "-- Brak zespołu --",
            Selected = true
        });

        ViewBag.teams = teamList;
        return View();
    }

    [Authorize(Roles = "Hr, Ceo")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddAssignmentCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }

        await _sender.Send(command);

        try
        {
            var team = await _sender.Send(new GetTeamForUserQuery());
            return RedirectToAction("EmployersInTeam", "Team", new { TeamId = team.Id, TeamName = team.Name });

        }
        catch(BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("AllAssignments");
        }
    }

    [Authorize(Roles = "Hr, Ceo")]
    [HttpGet("AddAssignmentToTeam/{id}")]
    public async Task<IActionResult> AddAssignmentToTeam(Guid id)
    {
        var teams = await _sender.Send(new GetAllTeamsQuery());
        ViewBag.teams = teams
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            })
            .ToList();
        ViewBag.AssignmentId = id;
        return View();
    }

    [Authorize(Roles = "Hr, Ceo")]
    [HttpPost("AddAssignmentToTeam/{id}")]
    public async Task<IActionResult> AddAssignmentToTeam(AddAssignmentToTeamCommand command)
    {
        if (!ModelState.IsValid)
        {
            var teams = await _sender.Send(new GetAllTeamsQuery());
            ViewBag.teams = teams
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                })
                .ToList();
            ViewBag.AssignmentId = command.AssignmentId;
            return View(command);
        }
        try
        {
            await _sender.Send(command);
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            RedirectToAction("Index", "Departments");
        }
        return RedirectToAction("Index", "Departments");
    }

    [HttpGet("AllAssignments")]
    public async Task<IActionResult> AllAssignments(string filter = "active")
    {
        List<AssignmentDTO> assignments;
        string title;

        ViewBag.FilterOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Free Assignments", Value = "free", Selected = filter == "free" },
            new SelectListItem { Text = "Active Assignments", Value = "assigned", Selected = filter == "active" },
        };

        switch (filter)
        {
            case "free":
                assignments = await _sender.Send(new GetFreeAssignmentsQuery());
                title = "Free Assignments";
                break;
            case "active":
            default:
                assignments = await _sender.Send(new GetActiveAssignmentsQuery());
                title = "All Assignments";
                break;
        }

        ViewBag.Title = title;
        ViewBag.Filter = filter;

        return View("AllAssignments", assignments);
    }

    [Authorize(Roles = "Hr, Ceo")]
    [HttpGet("Edit/{assignmentId}")]
    public async Task<IActionResult> Edit(Guid assignmentId)
    {
        try
        {
            var assignment = await _sender.Send(new GetAssignmentByIdQuery(assignmentId));
            return View(assignment);
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [Authorize(Roles = "Hr, Ceo")]
    [HttpPost("Edit/{assignmentId}")]
    public async Task<IActionResult> Edit(Guid assignmentId, EditAssignmentCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            await _sender.Send(command);

            return RedirectToAction("AllAssignments");
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [Authorize(Roles = "Hr, Ceo, TeamLeader")]
    [HttpGet("{TeamId}/Complete/{AssignmentId}")]
    public async Task<IActionResult> Complete(Guid TeamId, Guid AssignmentId)
    {
        return await CompleteTask(TeamId,AssignmentId);
    }

    [Authorize(Roles = "Hr, Ceo, TeamLeader")]
    [HttpPost("{TeamId}/Complete/{AssignmentId}")]
    public async Task<IActionResult> CompleteTask(Guid TeamId, Guid AssignmentId)
    {
        try
        {
            await _sender.Send(new CompleteAssignmentCommand(AssignmentId));
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("Index", new { TeamId });
        }

        return RedirectToAction(
            actionName: "AddTaskRate",
            controllerName: "EmployeeRate",
            routeValues: new { TeamId });
    }
}
