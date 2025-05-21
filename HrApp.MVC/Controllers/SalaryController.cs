using HrApp.Application.Salary.Command.AddPaid;
using HrApp.Application.Salary.Command.UpdatePaid;
using HrApp.Application.Salary.Query.GetById;
using HrApp.Application.Salary.Query.GetByUserId;
using HrApp.Application.Users.Query.GetUserByEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace HrApp.MVC.Controllers;

[Route("salary")]
public class SalaryController : Controller
{
    private readonly ISender _sender;

    public SalaryController(ILogger<SalaryController> logger, ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("addpaid")]
    public IActionResult AddPaid()
    {
        return View();
    }

    [HttpPost("addpaid")]
    public async Task<IActionResult> AddPaid(AddPaidCommand command)
    {

        if (!ModelState.IsValid)
            return View(command);

        var user = await _sender.Send(new GetUserByEmailQuery(command.Email));
        command.UserId = user.Id;
        await _sender.Send(command);
        return RedirectToAction("AddPaid");
    }

    [HttpGet("editpaid/{useremail}")]
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

    [HttpPost("editpaid/{userid}")]
    public async Task<IActionResult> EditPaid(UpdatePaidCommand command)
    {
        var user = await _sender.Send(new GetUserByEmailQuery(command.Email));
        command.UserId = user.Id;
        if (!ModelState.IsValid)
            return View("EditPaid", command);

        await _sender.Send(command);
        return RedirectToAction("Index", "Departments");
    }
}
