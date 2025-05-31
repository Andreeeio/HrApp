using HrApp.Application.Calendars.Command.CreateCalendarEvent;
using HrApp.Application.Calendars.Query.GetGoogleCalendarEvents;
using HrApp.Application.GoogleOAuthTokens.Query.ExchangeCodeForToken;
using HrApp.Application.GoogleOAuthTokens.VerifyOAuthToken;
using HrApp.Application.Interfaces;
using HrApp.Application.Services;
using HrApp.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HrApp.MVC.Controllers;

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
    [HttpGet("Add")]
    public async Task<IActionResult> AddEvent()
    {
        try
        {
            ViewBag.IsConnected = await _sender.Send(new VerifyOAuthTokenQuery());
        }
        catch (NotFoundAuthOTokenException ex)
        {
            ViewBag.IsConnected = false;
        }
        return View();
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddEvent(CreateCalendarEventCommand command)
    {
        try
        {
            await _sender.Send(command);
        }
        catch(NotFoundAuthOTokenException ex)
        {
            return RedirectToAction("events");
        }
        return RedirectToAction("events");
    }

    [HttpGet("Callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        await _sender.Send(new ExchangeCodeForTokenQuery { Code = code });
        return RedirectToAction("Add");
    }

    [HttpGet("events")]
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
