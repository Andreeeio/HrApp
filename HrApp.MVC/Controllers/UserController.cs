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
using HrApp.Application.UserIpAddresses.Command.AddUserIpAddress;
using HrApp.Application.Users.Command.ImportUsersFromExcel;
using HrApp.Application.Users.Command.EditUser;
using HrApp.Application.Users.Query.GetUserById;
using Microsoft.AspNetCore.Authorization;
using HrApp.Application.Users.Command.FirstLoginUser;
using HrApp.Application.UserIpAddresses.Command.DeleteUserIpAddress;
using NuGet.Common;
using HrApp.Domain.Constants;
using HrApp.Application.Users.Query.GetAllUsers;

namespace HrApp.MVC.Controllers;


[Route("User")]
public class UserController : Controller
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = "Hr,Ceo")]
    [HttpGet("Create")]
    public IActionResult CreateUser()
    {
        return View();
    }

    [Authorize(Roles = "Hr,Ceo")]
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(AddUserCommand request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            await _sender.Send(request);
        }
        catch(BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }

        return View("Index");
    }

    [HttpGet("Login")]
    public IActionResult LoginUser()
    {
        return View();
    }

    [HttpPost("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginUser(LoginUserQuery request)
    {
        if (!ModelState.IsValid)
            return View(request);

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
                return RedirectToAction("Verf", "Authorization");
            }
            else if(token.Item2 == 2)
            {
                return RedirectToAction("AddVerf", "Authorization");
            }
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }
        catch (FirstLoginException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }

        return RedirectToAction("Index");
    }

    [HttpGet("FirstLogin")]
    public IActionResult FirstLoginUser()
    {
        return View();
    }

    [HttpPost("FirstLogin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FirstLoginUser(FirstLoginUserCommand request)
    {
        if (!ModelState.IsValid)
            return View(request);

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
        catch (FirstLoginException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("LoginUser");
        }

        return RedirectToAction("Index");
    }

    [HttpGet("Logout")]
    public IActionResult LogoutGet()
    {
        return Logout();
    }

    [HttpPost("Logout")]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt_token");
        return RedirectToAction("LoginUser");
    }

    [Authorize]
    [HttpGet("CurrentUser")]
    public async Task<IActionResult> CurrentUser()
    {
        return View(await _sender.Send(new GetDataFromTokenQuery()));
    }

    [Authorize]
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var user = await _sender.Send(new GetDataFromTokenQuery());

        if(user!.IsInRole(Roles.Ceo.ToString()) || user.IsInRole(Roles.Hr.ToString()))
            return RedirectToAction("IndexAll");
        

        ViewBag.UserId = user!.id;
        var workLogs = await _sender.Send(new GetWorkLogQuery(Guid.Parse(user.id))); 
        var todayWorkLog = workLogs.FirstOrDefault(wl => wl.StartTime.Date == DateTime.UtcNow.Date);
        return View(todayWorkLog);
    }

    [Authorize]
    [HttpGet("Index/All")]
    public async Task<IActionResult> IndexAll()
    {
        var users = await _sender.Send(new GetAllUsersQuery());
        return View(users);
    }

    [Authorize]
    [HttpGet("{encodedName}/Details")]
    public async Task<IActionResult> Details(string encodedName)
    {
        try
        {
            var dto = await _sender.Send(new GetUserByEmailQuery(encodedName));
            ViewBag.UserId = dto!.Id;
            Console.WriteLine(dto.Id);
            return View(dto);
        }
        catch (BadRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    [Authorize(Roles = "Hr,Ceo, TeamLeader")]
    [HttpGet("{userId}/Manage")]
    public async Task<IActionResult> Manage(Guid userId)
    {
        var user = await _sender.Send(new GetUserByIdQuery(userId));
        if (user == null)
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction("Index");
        }
        return View(user);
    }

    [Authorize(Roles = "Hr,Ceo, TeamLeader")]
    [HttpPost("{UserId}/DeleteUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(Guid UserId)
    {
        var command = new DeleteUserCommand
        {
            UserId = UserId
        };
        await _sender.Send(command);
        return RedirectToAction("Logout");
    }

    [Authorize(Roles = "Hr,Ceo, TeamLeader")]
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

    [Authorize(Roles = "Hr,Ceo, TeamLeader")]
    [HttpPost("{email}/EditRole")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRole(string email, List<string> selectedRoles)
    {
        if (selectedRoles == null)
        {
            selectedRoles = new List<string>();
            selectedRoles.Add("User");
        }

        try
        {
            await _sender.Send(new ChangeRolesCommand(email)
            {
                SelectedRoles = selectedRoles
            });
        }
        catch(BadRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index");
        }

        return RedirectToAction("Details", new { email }); 
    }

    [Authorize]
    [HttpPost("CreateNewIp")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateNewIp()
    {
        try
        {
            var jwt = await _sender.Send(new AddUserIpAddressCommand());

            Response.Cookies.Append("jwt_token", jwt, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });

            TempData["SuccessMessage"] = "IP verification created.";
        }
        catch (BadRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction("CurrentUser");
    }

    [Authorize]
    [HttpPost("DeleteIp")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteIp()
    {
        try
        {
            var jwt = await _sender.Send(new DeleteUserIpAddressCommand());
            Response.Cookies.Append("jwt_token", jwt, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });

            TempData["SuccessMessage"] = "IP verification deleted.";

        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction("CurrentUser");
    }

    [Authorize(Roles = "Hr,Ceo")]
    [HttpGet("Import")]
    public IActionResult ImportUsers()
    {
        return View();
    }

    [Authorize(Roles = "Hr,Ceo")]
    [HttpPost("Import")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportUsers(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("file", "Please select a file.");
            return View();
        }
        try
        {
            await _sender.Send(new ImportUsersFromExcelCommand
            {
                ExcelFile = file
            });
        }
        catch(BadRequestException ex)
        {
            ModelState.AddModelError("file", ex.Message);
            return View();
        }
        catch(InvalidOperationException ex)
        {
            ModelState.AddModelError("file", ex.Message);
            return View();
        }


        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Hr,Ceo")]
    [HttpGet("{id}/Edit")]
    public async Task<IActionResult> EditUser(Guid id)
    {
        var user = await _sender.Send(new GetUserByIdQuery(id));

        if (user == null)
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction("Index");
        }

        return View(new EditUserCommand
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        });
    }

    [Authorize(Roles = "Hr,Ceo")]
    [HttpPost("{id}/Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(EditUserCommand command)
    {
        if (!ModelState.IsValid)
            return View(command);

        try
        {
            await _sender.Send(command);
        }
        catch (BadRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index");
        }

        return RedirectToAction("Details", new { encodedName = command.Email });
    }

}
