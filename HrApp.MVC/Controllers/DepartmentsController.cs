using Microsoft.AspNetCore.Mvc;
using MediatR;
using HrApp.Application.Department.Query.GetAllDepartments;
using System.Threading.Tasks;
using HrApp.Application.Department.Command.AddDepartment;

namespace HrApp.MVC.Controllers;

public class DepartmentsController : Controller
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    // GET: Departments
    public async Task<IActionResult> Index()
    {
        var departments = await _mediator.Send(new GetAllDepartmentsQuery());
        return View(departments);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddDepartmentCommand command)
    {
        if (!ModelState.IsValid)
        {
            // Handle the command to create a department
            return View(command);
        }

        await _mediator.Send(command);
        return RedirectToAction("Index"); ;
    }


}
