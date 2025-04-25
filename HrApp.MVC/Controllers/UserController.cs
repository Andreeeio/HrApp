using Azure.Core;
using HrApp.Application.Users.Command.AddUser;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HrApp.MVC.Controllers;

[Route("user")]
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly ISender _sender;

    public UserController(ILogger<UserController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpGet("create")]
    public IActionResult CreateUser()
    {
        _logger.LogInformation("Creating new user with email");

        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(AddUserCommand request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }
        _logger.LogInformation("Creating new user with email {Email}", request.Email);

        await _sender.Send(request);

        return RedirectToAction(nameof(Index));
    }
}
