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

        return View();
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
            Response.Cookies.Append("jwt_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(10)
            });
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }

        return RedirectToAction("CurrentUser");
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt_token");
        return RedirectToAction("LoginUser");
    }

    [HttpGet("currentuser")]
    public async Task<IActionResult> CurrentUser()
    {
        return View(await _sender.Send(new GetDataFromTokenQuery()));
    }

    [HttpGet("index")]
    public IActionResult Index()
    {
        _logger.LogInformation("Getting all users");
        return View();
    }
}
