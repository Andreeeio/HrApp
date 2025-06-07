using HrApp.Application.EmploymentHistories.Command.AddEmploymentHistory;
using HrApp.Application.EmploymentHistories.Query.GetEmploymentHistoryForUser;
using HrApp.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrApp.MVC.Controllers;

[Authorize]
[Route("EmploymentHistory")]
public class EmploymentHistoryController : Controller
{
    private readonly ISender _sender;
    public EmploymentHistoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetEmpHist(Guid userId)
    {
        var employmentHistory = await _sender.Send(new GetEmploymentHistoryForUserQuery(userId));
        return View(employmentHistory);
    }

    [Authorize(Roles = "Hr, Ceo")]
    [HttpGet("Create")]
    public IActionResult CreateEmpHist([FromQuery]string? email = null)
    {
        ViewBag.Email = email;
        return View(new AddEmploymentHistoryCommand());
    }

    [Authorize(Roles = "Hr, Ceo")]
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEmpHist(AddEmploymentHistoryCommand command)
    {
        if (!ModelState.IsValid)
            return View(command);

        try
        {
            await _sender.Send(command);
            TempData["Success"] = "History has been added.";
            return RedirectToAction("Index", "Departments");
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(command);
    }
}
