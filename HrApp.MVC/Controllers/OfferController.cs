using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HrApp.Application.Teams.Query.GetAllTeams;
using HrApp.Application.Offer.Query;
using HrApp.Application.Offer.Command.CreateOffer;
using HrApp.Application.Offer.Query.GetAllOffers;
using HrApp.Application.Offer.Command.CreateCandidate;
using HrApp.Application.Offer.Command.CreateJobApplication;
using HrApp.Application.Offer.DTO;
using HrApp.Application.Offer.Query.ShowCandidates;

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
    [HttpGet("Apply/{id}")]
    public IActionResult Apply(Guid id)
    {
        return View(new ApplyForOfferModel { OfferId = id });
    }

    [HttpPost("Apply/{id}")]
    public async Task<IActionResult> Apply(ApplyForOfferModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var candidateId = await _sender.Send(new CreateCandidateCommand
        {
            Name = model.Name,
            Surname = model.Surname,
            Email = model.Email,
            HomeNumber = model.HomeNumber,
            Street = model.Street,
            City = model.City
        });

        await _sender.Send(new CreateJobApplicationCommand
        {
            OfferId = model.OfferId,
            CandidateId = candidateId,
            CvLink = model.CvLink,
            ApplicationDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = "Pending"
        });

        return RedirectToAction("Index");
    }

    [HttpGet("ShowCandidates/{id}")]
    public async Task<IActionResult> ShowCandidates(Guid id)
    {
        var candidates = await _sender.Send(new ShowCandidatesQuery(id));
        return View(candidates);
    }

}
