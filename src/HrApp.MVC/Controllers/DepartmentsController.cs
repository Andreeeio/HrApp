using Microsoft.AspNetCore.Mvc;
using MediatR;
using HrApp.Application.Department.Query.GetAllDepartments;
using HrApp.Application.Department.Command.AddDepartment;
using HrApp.Application.Department.Command.DeleteDepartment;
using HrApp.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace HrApp.MVC.Controllers;

[Authorize(Roles = "Junior, Mid, Senior, TeamLeader, Hr, Ceo")]
[Route("Departments")]
public class DepartmentsController : Controller
{
    private readonly ISender _sender;

    public DepartmentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var departments = await _sender.Send(new GetAllDepartmentsQuery());
        return View(departments);
    }

    [Authorize(Roles = "Ceo")]
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Ceo")]
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddDepartmentCommand command)
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
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(command);
        }

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Ceo")]
    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        var command = new DeleteDepartmentCommand
        {
            DepartmentId = id
        };

        try
        {
            await _sender.Send(command);
        }
        catch(BadRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
}
