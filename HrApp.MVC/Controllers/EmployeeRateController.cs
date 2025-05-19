using HrApp.Application.EmployeeRates.Command.AddTaskRate;
using HrApp.Application.EmployeeRates.Query.GetEmployeeToRate;
using HrApp.Application.Teams.Query.GetEmployersInTeam;
using HrApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HrApp.MVC.Controllers;

[Route("employeeRate")]
public class EmployeeRateController : Controller
{
    private readonly ISender _sender;
    public EmployeeRateController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{teamId}")]
    public async Task<IActionResult> AddTaskRate(Guid teamId)
    {
        var employess = await _sender.Send(new GetEmployersToRateQuery(teamId));
        return View(employess);
    }
    [HttpPost("{teamId}")]
    public async Task<IActionResult> AddTaskRate(Guid teamId, AddTaskRatesCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }
        await _sender.Send(command);
        return RedirectToAction("Index", "Assignment", new { TeamId = teamId });
    }
}
