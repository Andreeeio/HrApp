using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HrApp.Application.Teams.Query.GetAllTeams;
using HrApp.Application.Offer.Query;
using HrApp.Application.Offer.Command.CreateOffer;
using HrApp.Application.Offer.Query.GetAllOffers;

namespace HrApp.MVC.Controllers;

[Route("offer")]
public class OfferController : Controller
{
    private readonly ISender _sender;

    public OfferController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var offers = await _sender.Send(new GetAllOffersQuery());
        return View(offers);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        var teams = await _sender.Send(new GetAllTeamsQuery());
        ViewBag.Teams = new SelectList(teams, "Id", "Name");
        return View();
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateOfferCommand command)
    {
        command.AddDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (!ModelState.IsValid)
        {
            var teams = await _sender.Send(new GetAllTeamsQuery());
            ViewBag.Teams = new SelectList(teams, "Id", "Name");
            return View(command);
        }

        await _sender.Send(command);
        return RedirectToAction("Index");
    }

    //[HttpGet("Details/{id}")]
    //public async Task<IActionResult> Details(Guid id)
    //{
    //    var offer = await _sender.Send(new GetOfferByIdQuery(id));
    //    if (offer == null) return NotFound();

    //    return View(offer);
    //}
}
