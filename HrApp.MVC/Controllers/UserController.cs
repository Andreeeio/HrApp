using Azure.Core;
using HrApp.Application.Users.Command.AddUser;
using HrApp.Application.Users.Query.GetDataFromToken;
using HrApp.Application.Users.Query.LoginUser;
using HrApp.Domain.Constants;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using HrApp.Application.Users.Query.GetUserByEmail;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using HrApp.Application.WorkLog.Query.GetWorkLog;
using HrApp.Application.Teams.Command.DeleteTeam;
using HrApp.Application.Users.Command.DeleteUser;

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
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(AddUserCommand request)
    {
        if (!ModelState.IsValid)
            return View(request);

        await _sender.Send(request);

        return View("LoginUser");
    }

    [HttpGet("login")]
    public IActionResult LoginUser()
    {
        _logger.LogInformation("User trying to log in");
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginUserQuery request)
    {
        if (!ModelState.IsValid)
            return View(request);

        _logger.LogInformation("User login in");
        try
        {
            var token = await _sender.Send(request);

            Response.Cookies.Append("jwt_token", token.Item1, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(10)
            });

            if(token.Item2 == 1)
            {
                return RedirectToAction("verf", "Authorization");
            }
            else if(token.Item2 == 2)
            {
                return RedirectToAction("addverf", "Authorization");
            }
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }

        return RedirectToAction("Index");
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt_token");
        return RedirectToAction("LoginUser");
    }
    [HttpGet("logout")]
    public IActionResult LogoutGet()
    {
        return Logout();
    }

    [HttpGet("currentuser")]
    public async Task<IActionResult> CurrentUser()
    {
        return View(await _sender.Send(new GetDataFromTokenQuery()));
    }

    [HttpGet("index")]
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Getting current users");
        var user = await _sender.Send(new GetDataFromTokenQuery());
        ViewBag.UserId = user.id;
        var workLogs = await _sender.Send(new GetWorkLogQuery(Guid.Parse(user.id))); // Fetch work logs
        var todayWorkLog = workLogs.FirstOrDefault(wl => wl.StartTime.Date == DateTime.UtcNow.Date);
        return View(todayWorkLog); // Pass work logs to the view
    }

    [Route("User/{encodedName}/Details")]
    public async Task<IActionResult> Details(string encodedName)
    {
        var dto = await _sender.Send(new GetUserByEmailQuery(encodedName));
        return View(dto);
    }

    [HttpGet("{UserId}/DeleteUser")]
    public async Task<IActionResult> DeleteUser(Guid UserId)
    {
        var command = new DeleteUserCommand
        {
            UserId = UserId
        };
        await _sender.Send(command);
        return RedirectToAction("Logout");
    }
}
