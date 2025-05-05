using HrApp.Application.Department.Query.GetAllDepartments;
using HrApp.Application.Teams.Command.AddTeam;
using HrApp.Application.Teams.Query.GetEmployersInTeam;
using HrApp.Application.Teams.Query.GetTeamForDepartment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HrApp.MVC.Controllers;

[Route("team")]
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

    [HttpGet("{id}/employers")]
    public async Task<IActionResult> EmployersInTeam(Guid id)
    {
        var employers = await _sender.Send(new GetEmployersInTeamQuery(id));
        return View(employers);
    }

    public async Task<IActionResult> Create()
    {
        var departments = await _sender.Send(new GetAllDepartmentsQuery());
        ViewBag.Departments = new SelectList(departments, "Id", "Name");
        return View();
    }
    //[HttpGet("{id}/Create")]
    //public async Task<IActionResult> Create(Guid id)
    //{
    //    var departments = await _sender.Send(new GetAllDepartmentsQuery());
    //    ViewBag.Departments = new SelectList(departments, "Id", "Name");

    //    // Set the default DepartmentId
    //    var model = new AddTeamCommand
    //    {
    //        DepartmentId = id // Replace with your desired default ID
    //    };

    //    return View(model);
    //}

    [HttpPost]
    public async Task<IActionResult> Create(AddTeamCommand command)
    {
        if (!ModelState.IsValid)
        {
            // Handle the command to create a team
            return View(command);
        }
        await _sender.Send(command);
        return RedirectToAction("Index");
    }
}