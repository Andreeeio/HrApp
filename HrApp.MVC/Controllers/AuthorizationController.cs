using HrApp.Application.Authorizations.Command.Add2FA;
using HrApp.Application.Authorizations.Command.CreateNewCode;
using HrApp.Application.Authorizations.Command.Validate2FA;
using HrApp.Application.Users.Query.LoginUser;
using HrApp.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HrApp.MVC.Controllers;

[Authorize(Roles = "TeamLeader, Hr, Ceo")]
[Route("Authorization")]
public class AuthorizationController : Controller
{
    private readonly ISender _sender;
    public AuthorizationController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("Verf")]
    public async Task<IActionResult> Verf2FA()
    {
        await _sender.Send(new CreateNewCodeCommand());
        return View(new Validate2FARequest());
    }

    [HttpPost("Verf")]
    public async Task<IActionResult> Verf2FA(Validate2FARequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var token = await _sender.Send(request);

            if (Request.Cookies.ContainsKey("jwt_token"))
            {
                Response.Cookies.Delete("jwt_token");
            }

            Response.Cookies.Append("jwt_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(10)
            });

            return RedirectToAction("currentuser", "User");
        }
        catch (No2FAException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("AddVerf");
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(request);
    }


    [HttpGet("AddVerf")]
    public IActionResult AddVerf2FA()
    {
        return View(new Add2FARequest());
    }

    [HttpPost("AddVerf")]
    public async Task<IActionResult> AddVerf2FA(Add2FARequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            await _sender.Send(request);
            return RedirectToAction("Verf", "Authorization");
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(request);
    }
}
