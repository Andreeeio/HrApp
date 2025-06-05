using HrApp.Application.EmployeeRates.Command.AddTaskRate;
using HrApp.Application.EmployeeRates.Query.GetEmployeeToRate;
using HrApp.Application.EmployeeRates.Query.GetRatesForUser;
using HrApp.Application.Teams.Query.GetEmployersInTeam;
using HrApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HrApp.MVC.Controllers;

[Authorize]
[Route("EmployeeRate")]
public class EmployeeRateController : Controller
{
    private readonly ISender _sender;
    public EmployeeRateController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = "TeamLeader")]
    [HttpGet("{teamId}")]
    public async Task<IActionResult> AddTaskRate(Guid teamId)
    {
        var employess = await _sender.Send(new GetEmployersToRateQuery(teamId));
        return View(employess);
    }

    [Authorize(Roles = "TeamLeader")]
    [HttpPost("{teamId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTaskRate(Guid teamId, AddTaskRatesCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }
        await _sender.Send(command);
        TempData["Success"] = "Task rates added successfully.";
        return RedirectToAction("Index", "Assignment", new { TeamId = teamId });
    }

    [HttpGet("Rates/{userId}")]
    public async Task<IActionResult> GetRatesForUser(Guid userId)
    {
        var query = await _sender.Send(new GetRatesForUserQuery(userId));

        var rates = query.Item1;
        ViewBag.AverageRate = query.Item2;

        return View(rates);
    }
}
