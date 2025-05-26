using HrApp.Application.EmploymentHistories.Command.AddEmploymentHistory;
using HrApp.Application.EmploymentHistories.Query.GetEmploymentHistoryForUser;
using HrApp.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HrApp.MVC.Controllers;

[Route("employmentHistory")]
public class EmploymentHistoryController : Controller
{
    private readonly ISender _sender;
    public EmploymentHistoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{UserId}")]
    public async Task<IActionResult> GetEmpHist(Guid UserId)
    {
        var employmentHistory = await _sender.Send(new GetEmploymentHistoryForUserQuery(UserId));
        return View(employmentHistory);
    }

    [HttpGet("Create")]
    public IActionResult CreateEmpHist()
    {
        return View(new AddEmploymentHistoryCommand());
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateEmpHist(AddEmploymentHistoryCommand command)
    {
        if (!ModelState.IsValid)
            return View(command);

        try
        {
            await _sender.Send(command);
            TempData["Success"] = "Historia zatrudnienia została dodana.";
            return RedirectToAction("Index", "Home");
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Wystąpił błąd: " + ex.Message);
        }

        return View(command);
    }
}
