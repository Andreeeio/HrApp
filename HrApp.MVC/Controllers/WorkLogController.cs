using HrApp.Application.Users.Query.GetDataFromToken;
using HrApp.Application.WorkLog.Command.AddWorkLog;
using HrApp.Application.WorkLog.Command.UpdateWorkLog;
using HrApp.Application.WorkLog.Query.GetWorkLog;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("worklog")]
public class WorkLogController : Controller
{
    private readonly ISender _sender;

    public WorkLogController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{UserId}/worklogs")]
    public async Task<IActionResult> GetWorkLogs(Guid UserId)
    {
        var workLogs = await _sender.Send(new GetWorkLogQuery(UserId));
        var user = await _sender.Send(new GetDataFromTokenQuery());
        ViewBag.UserEmail = user.email;
        return View(workLogs);
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartWorkLog(Guid userId)
    {
        // Sprawdź, czy istnieje już WorkLog dla danego dnia
        var existingWorkLogs = await _sender.Send(new GetWorkLogQuery(userId));
        if (existingWorkLogs.Any(wl => wl.StartTime.Date == DateTime.UtcNow.Date))
        {
            return BadRequest("WorkLog for today already exists.");
        }

        // Utwórz nowy WorkLog
        var command = new AddWorkLogCommand
        {
            UserId = userId,
            StartTime = DateTime.UtcNow
        };

        await _sender.Send(command);
        return RedirectToAction("Index", "User");
    }

    [HttpPost("end")]
    public async Task<IActionResult> EndWorkLog(Guid userId)
    {
        // Pobierz WorkLog dla danego dnia
        var existingWorkLogs = await _sender.Send(new GetWorkLogQuery(userId));
        var todayWorkLog = existingWorkLogs.FirstOrDefault(wl => wl.StartTime.Date == DateTime.UtcNow.Date && wl.EndTime == null);

        if (todayWorkLog == null)
        {
            return BadRequest("No active WorkLog found for today.");
        }

        // Oblicz czas zakończenia i przepracowane godziny
        todayWorkLog.EndTime = DateTime.UtcNow;
        todayWorkLog.Hours = (int)(todayWorkLog.EndTime.Value - todayWorkLog.StartTime).TotalHours;

        // Wyślij komendę aktualizacji
        var command = new UpdateWorkLogCommand
        {
            Id = todayWorkLog.Id,
            EndTime = todayWorkLog.EndTime,
            Hours = todayWorkLog.Hours
        };

        await _sender.Send(command);
        return RedirectToAction("Index", "User");
    }

    //[HttpGet("report")]
    //public async Task<IActionResult> GenerateReport(Guid userId)
    //{
    //    var report = await _sender.Send(new GenerateWorkedHoursReportQuery(userId));
    //    return View(report);
    //}
}