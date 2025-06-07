using HrApp.Application.Calendars.Command.CreateCalendarEvent;
using HrApp.Application.Calendars.Query.GetGoogleCalendarEvents;
using HrApp.Application.GoogleOAuthTokens.Query.ExchangeCodeForToken;
using HrApp.Application.GoogleOAuthTokens.Query.VerifyOAuthToken;
using HrApp.Application.Interfaces;
using HrApp.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrApp.MVC.Controllers;

[Authorize]
[Route("Calendar")]
public class CalendarController : Controller
{
    private readonly IGoogleAuthService _auth;
    private readonly ISender _sender;

    public CalendarController(IGoogleAuthService auth, ISender sender)
    {
        _auth = auth;
        _sender = sender;
    }

    [HttpGet("connect")]
    public IActionResult Connect()
    {
        var url = _auth.GetAuthorizationUrl();
        return Redirect(url);
    }

    [Authorize(Roles = "TeamLeader,Hr,Ceo")]
    [HttpGet("Add")]
    public async Task<IActionResult> AddEvent()
    {
        try
        {
            ViewBag.IsConnected = await _sender.Send(new VerifyOAuthTokenQuery());
        }
        catch (NotFoundAuthOTokenException)
        {
            ViewBag.IsConnected = false;
        }
        return View();
    }

    [Authorize(Roles = "TeamLeader,Hr,Ceo")]
    [HttpPost("Add")]
    public async Task<IActionResult> AddEvent(CreateCalendarEventCommand command)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IsConnected = true;
            return View(command);
        }

        try
        {
            await _sender.Send(command);
        }
        catch(NotFoundAuthOTokenException)
        {
            return RedirectToAction("Events");
        }
        return RedirectToAction("Events");
    }

    [HttpGet("Callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        await _sender.Send(new ExchangeCodeForTokenQuery { Code = code });
        return RedirectToAction("Add");
    }

    [HttpGet("Events")]
    public async Task<IActionResult> Events()
    {
        var events = await _sender.Send(new GetGoogleCalendarEventsQuery());
        return View(events);
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Events));
    }
}
