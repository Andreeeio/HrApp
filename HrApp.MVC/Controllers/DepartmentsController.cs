using Microsoft.AspNetCore.Mvc;
using MediatR;
using HrApp.Application.Department.Query.GetAllDepartments;

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

}
