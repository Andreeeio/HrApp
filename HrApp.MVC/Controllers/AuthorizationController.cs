using HrApp.Application.Authorizations.Command.Add2FA;
using HrApp.Application.Authorizations.Command.Validate2FA;
using HrApp.Application.Users.Query.LoginUser;
using HrApp.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HrApp.MVC.Controllers;

[Route("authorization")]
public class AuthorizationController : Controller
{
    private readonly ILogger<AuthorizationController> _logger;
    private readonly ISender _sender;
    public AuthorizationController(ISender sender, ILogger<AuthorizationController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    [HttpGet("verf")]
    public IActionResult Verf2FA()
    {
        return View(new Validate2FARequest());
    }

    [HttpPost("verf")]
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
         _logger.LogError(ex.ToString());
            return RedirectToAction("addverf", "Authorization");
        }
        catch (BadRequestException ex)
        {
            _logger.LogError(ex.ToString());
            ModelState.AddModelError(string.Empty, "Niepoprawny kod weryfikacyjny.");
        }

        return View(request);
    }


    [HttpGet("addverf")]
    public IActionResult AddVerf2FA()
    {
        return View(new Add2FARequest());
    }

    [HttpPost("addverf")]
    public async Task<IActionResult> AddVerf2FA(Add2FARequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            await _sender.Send(request);
            TempData["Success"] = "Verification code sent to your email.";
            return RedirectToAction("verf", "Authorization");
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(request);
    }
}
