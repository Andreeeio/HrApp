using HrApp.Application.Users.Command.AddUser;
using HrApp.Application.Users.Query.GetDataFromToken;
using HrApp.Application.Users.Query.LoginUser;
using HrApp.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HrApp.Application.Users.Query.GetUserByEmail;
using HrApp.Application.WorkLog.Query.GetWorkLog;
using HrApp.Application.Users.Command.DeleteUser;
using HrApp.Application.Users.Query.GetRoleForUser;
using HrApp.Application.Users.Command.ChangeRoles;
using System.Threading.Tasks;
using HrApp.Application.UserIpAddresses.Command.AddUserIpAddress;
using HrApp.Application.Users.Command.ImportUsersFromExcel;
using HrApp.Application.Users.Command.EditUser;
using HrApp.Application.Users.Query.GetUserById;

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
        var workLogs = await _sender.Send(new GetWorkLogQuery(Guid.Parse(user.id))); 
        var todayWorkLog = workLogs.FirstOrDefault(wl => wl.StartTime.Date == DateTime.UtcNow.Date);
        return View(todayWorkLog);
    }

    [Route("User/{encodedName}/Details")]
    public async Task<IActionResult> Details(string encodedName)
    {
        var dto = await _sender.Send(new GetUserByEmailQuery(encodedName));
        ViewBag.UserId = dto.Id;
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

    [HttpGet("{email}/EditRole")]
    public async Task<IActionResult> EditRole(string email)
    {
        var dto = await _sender.Send(new GetRoleForUserCommand(email));
        List<string> all = new()
        {
            "User", "Junior", "Mid", "Senior", "TeamLeader", "Hr"
        };

        ViewBag.AllRoles = all;

        return View(dto);
    }

    [HttpPost("{email}/EditRole")]
    public async Task<IActionResult> EditRole(string email, List<string> selectedRoles)
    {
        if (selectedRoles == null)
        {
            selectedRoles = new List<string>();
            selectedRoles.Add("User");
        }

        await _sender.Send(new ChangeRolesCommand(email)
        {
            SelectedRoles = selectedRoles
        });

        return RedirectToAction("Details", new { email }); 
    }

    [HttpGet("CreateNewIp")]
    public async Task<IActionResult> CreateNewIp()
    {
        try
        {
            await _sender.Send(new AddUserIpAddressCommand());

        }
        catch (BadRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Wystąpił nieoczekiwany błąd.";
        }

        return RedirectToAction("currentuser");
    }



    [HttpGet("import")]
    public IActionResult ImportUsers()
    {
        return View();
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportUsers(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("file", "Please select a file.");
            return View();
        }

        await _sender.Send(new ImportUsersFromExcelCommand
        {
            ExcelFile = file
        });

        return RedirectToAction("Index");
    }

    [HttpGet("{id}/Edit")]
    public async Task<IActionResult> EditUser(Guid id)
    {
        var user = await _sender.Send(new GetUserByIdQuery(id));
        return View(new EditUserCommand
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        });
    }

    [HttpPost("{id}/Edit")]
    public async Task<IActionResult> EditUser(EditUserCommand command)
    {
        if (!ModelState.IsValid)
            return View(command);

        await _sender.Send(command);
        return RedirectToAction("Details", new { encodedName = command.Email });
    }

}
