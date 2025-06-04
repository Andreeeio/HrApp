using HrApp.Application.SalaryHistories.Query.GetSalaryHistoryForUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrApp.MVC.Controllers;

[Authorize]
[Route("SalaryHistory")]
public class SalaryHistoryController : Controller
{
    private readonly ISender _sender;

    public SalaryHistoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetSalaryHistory(Guid userId, [FromQuery] int? number)
    {
        var salaryHistory = await _sender.Send(new GetSalaryHistoryForUserQuery(userId, number));
        if (salaryHistory == null) return NotFound();
        return View(salaryHistory);
    }

}
