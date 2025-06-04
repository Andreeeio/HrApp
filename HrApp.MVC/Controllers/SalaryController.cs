using HrApp.Application.Salary.Command.AddPaid;
using HrApp.Application.Salary.Command.UpdatePaid;
using HrApp.Application.Salary.Query.GetByUserId;
using HrApp.Application.Users.Query.GetUserByEmail;
using HrApp.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrApp.MVC.Controllers;

[Authorize(Roles = "Hr, Ceo")]
[Route("Salary")]
public class SalaryController : Controller
{
    private readonly ISender _sender;

    public SalaryController(ILogger<SalaryController> logger, ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("AddPaid")]
    public IActionResult AddPaid([FromQuery] string email = null)
    {
        ViewBag.Email = email;
        return View();
    }

    [HttpPost("AddPaid")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPaid(AddPaidCommand command)
    {
        if (!ModelState.IsValid)
            return View(command);

        try
        {
            await _sender.Send(command);
        }
        catch(BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(command);
        }

        return RedirectToAction("Index", "Departments");
    }

    [HttpGet("EditPaid/{useremail}")]
    public async Task<IActionResult> EditPaid(string useremail)
    {
        var user = await _sender.Send(new GetUserByEmailQuery(useremail));
        var paid = await _sender.Send(new GetPaidByUserIdQuery(user.Id)); 
        if (paid == null)
            return NotFound();

        var command = new UpdatePaidCommand
        {
            Id = paid.Id,
            UserId = paid.UserId,
            BaseSalary = paid.BaseSalary,
            Email = user.Email,
        };
        return View("EditPaid", command);
    }

    [HttpPost("EditPaid/{useremail}")]
    public async Task<IActionResult> EditPaid(UpdatePaidCommand command)
    {
        if (!ModelState.IsValid)
            return View("EditPaid", command);
        try
        {
            await _sender.Send(command);
        }
        catch(BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("EditPaid", command);
        }
        return RedirectToAction("Index", "Departments");
    }
}
