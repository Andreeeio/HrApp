using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HrApp.Domain.Entities;
using HrApp.Infrastructure.Presistance;
using MediatR;
using HrApp.Application.Department.Query.GetAllDepartments;

namespace HrApp.MVC.Controllers
{
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
}
