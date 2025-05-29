using HrApp.Application.Calendars.Query.GetGoogleCalendarEvents;
using HrApp.Application.GoogleOAuthTokens.Query.ExchangeCodeForToken;
using HrApp.Application.Interfaces;
using HrApp.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HrApp.MVC.Controllers;

[Route("Calendar")]
public class CalendarController : Controller
{
    private readonly IGoogleAuthService _auth;
    private readonly IMediator _mediator;

    public CalendarController(IGoogleAuthService auth, IMediator mediator)
    {
        _auth = auth;
        _mediator = mediator;
    }

    [HttpGet("connect")]
    public IActionResult Connect()
    {
        var url = _auth.GetAuthorizationUrl();
        return Redirect(url);
    }

    [HttpGet("Callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        await _mediator.Send(new ExchangeCodeForTokenQuery { Code = code });
        return RedirectToAction(nameof(Events));
    }

    [HttpGet("events")]
    public async Task<IActionResult> Events()
    {
        var events = await _mediator.Send(new GetGoogleCalendarEventsQuery());
        if (events == null)
        {
            ViewBag.IsConnected = false;
            View();
        }
        ViewBag.IsConnected = true;
        return View(events);
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Events));
    }
}
