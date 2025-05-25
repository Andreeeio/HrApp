using ClosedXML.Excel;
using HrApp.Application.Users.DTO;
using HrApp.Application.Users.Query.GetDataFromToken;
using HrApp.Application.Users.Query.GetUserByEmail;
using HrApp.Application.Users.Query.GetUserById;
using HrApp.Application.WorkLog.Command.AddWorkLog;
using HrApp.Application.WorkLog.Command.AddWorkLogExportHistory;
using HrApp.Application.WorkLog.Command.UpdateWorkLog;
using HrApp.Application.WorkLog.Query.GetWorkLog;
using HrApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HrApp.MVC.Controllers;

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
        ViewBag.UserId = Guid.Parse(user.id);
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
    [HttpGet("{UserId}/report")]
    public async Task<IActionResult> GenerateReport(Guid UserId)
    {
        // Pobierz dane WorkLog dla użytkownika
        var workLogs = await _sender.Send(new GetWorkLogQuery(UserId));

        // Grupuj dane według miesiąca i roku, sumując godziny
        var report = workLogs
            .GroupBy(wl => new { wl.StartTime.Year, wl.StartTime.Month })
            .Select(g => new WorkedHoursRaport
            {
                Id = Guid.NewGuid(),
                UserId = UserId,
                WorkedHours = g.Sum(wl => wl.Hours),
                MonthNYear = new DateOnly(g.Key.Year, g.Key.Month, 1)
            })
            .ToList();

        // Przekaż dane do widoku
        return View(report);
    }

    [HttpGet("{UserId}/export")]
    public async Task<IActionResult> ExportToExcel(Guid UserId)
    {
        var workLogs = await _sender.Send(new GetWorkLogQuery(UserId));
        var currentUser = await _sender.Send(new GetDataFromTokenQuery());
        var user = await _sender.Send(new GetUserByIdQuery(UserId));

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("WorkLogs");

        // Nagłówki
        worksheet.Cell(1, 1).Value = "Log ID";
        worksheet.Cell(1, 2).Value = "Start Time";
        worksheet.Cell(1, 3).Value = "End Time";
        worksheet.Cell(1, 4).Value = "Worked Hours";

        // Dane
        for (int i = 0; i < workLogs.Count; i++)
        {
            var log = workLogs[i];
            worksheet.Cell(i + 2, 1).Value = log.Id.ToString();
            worksheet.Cell(i + 2, 2).Value = log.StartTime.ToString("yyyy-MM-dd HH:mm");
            worksheet.Cell(i + 2, 3).Value = log.EndTime?.ToString("yyyy-MM-dd HH:mm") ?? "In Progress";
            worksheet.Cell(i + 2, 4).Value = log.Hours;
        }

        // Auto-fit kolumn
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        
        var exportRecord = new AddWorkLogExportHistoryCommand
        {
            ExportedByUserId = Guid.Parse(currentUser.id),
            ExportedForUserId = UserId,
            ExportDate = DateTime.UtcNow
        };

        await _sender.Send(exportRecord);

        return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"WorkLogs_{user.Email}.xlsx");
    }
}