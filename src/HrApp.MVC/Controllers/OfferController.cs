using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HrApp.Application.Teams.Query.GetAllTeams;
using HrApp.Application.Offer.Command.CreateOffer;
using HrApp.Application.Offer.Query.GetAllOffers;
using HrApp.Application.Offer.Command.CreateCandidate;
using HrApp.Application.Offer.Command.CreateJobApplication;
using HrApp.Application.Offer.DTO;
using HrApp.Application.Offer.Query.ShowCandidates;
using HrApp.Application.Offer.Command.UpdateJobApOffer;
using Microsoft.AspNetCore.Authorization;
using HrApp.Domain.Exceptions;

namespace HrApp.MVC.Controllers;

[Route("Offer")]
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

    [Authorize(Roles = "TeamLeader,Hr,Ceo")]
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        var teams = await _sender.Send(new GetAllTeamsQuery());
        ViewBag.Teams = new SelectList(teams, "Id", "Name");
        return View();
    }

    [Authorize(Roles = "TeamLeader,Hr,Ceo")]
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
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

    [HttpGet("Apply/{id}")]
    public IActionResult Apply(Guid id)
    {
        return View(new ApplyForOfferModel { OfferId = id });
    }

    [HttpPost("Apply/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(ApplyForOfferModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
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
                CvFile = model.CvFile,
                ApplicationDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Status = "Received"
            });

            return RedirectToAction("Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred while applying for the offer: {ex.Message}");
            return View(model);
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            return View(model);
        }
    }

    [HttpGet("ShowCandidates/{id}")]
    public async Task<IActionResult> ShowCandidates(Guid id)
    {
        var candidates = await _sender.Send(new ShowCandidatesQuery(id));
        return View(candidates);
    }

    [Authorize(Roles = "Hr,Ceo")]
    [HttpGet("Update/{jobApIp}")]
    public IActionResult Update(Guid jobApIp)
    {
        var model = new UpdateJobApOfferCommand(jobApIp);
        return View(model);
    }

    [Authorize(Roles = "Hr,Ceo")]
    [HttpPost("Update/{jobApIp}")]
    public async Task<IActionResult> Update(Guid jobApIp, UpdateJobApOfferCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }

        await _sender.Send(command);
        return RedirectToAction("Index"); 
    }
}
