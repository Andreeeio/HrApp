using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HrApp.MVC.Models;
using Microsoft.AspNetCore.Authentication;
using HrApp.Application.Interfaces;

namespace HrApp.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRaportService _raportService;

    public HomeController(ILogger<HomeController> logger, IRaportService raportService)
    {
        _logger = logger;
        _raportService = raportService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
