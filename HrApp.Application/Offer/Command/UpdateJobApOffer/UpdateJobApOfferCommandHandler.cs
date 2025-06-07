using HrApp.Application.Interfaces;
using HrApp.Domain.Constants;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Offer.Command.UpdateJobApOffer;

public class UpdateJobApOfferCommandHandler(ILogger<UpdateJobApOfferCommandHandler> logger,
    IOfferRepository offerRepository,
    IUserContext userContext,
    IEmailSender emailSender,
    ITeamAuthorizationService teamAuthorizationService) : IRequestHandler<UpdateJobApOfferCommand>
{
    private readonly ILogger<UpdateJobApOfferCommandHandler> _logger = logger;
    private readonly IOfferRepository _offerRepository = offerRepository;
    private readonly IUserContext _userContext = userContext;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ITeamAuthorizationService _teamAuthorizationService = teamAuthorizationService;
    public async Task Handle(UpdateJobApOfferCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateJobApOfferCommand for Offer ID");

        var user = _userContext.GetCurrentUser();

        if (user == null)
            throw new UnauthorizedException("User is not authenticated");

        if (!_teamAuthorizationService.Authorize(ResourceOperation.Create))
            throw new UnauthorizedException("User is not authenticated");

        var jobApplication = await _offerRepository.GetJobApplicationAsync(request.JobApId);
        if(jobApplication == null)
            throw new BadRequestException("Job application not found");

        jobApplication.Status = request.UpdatedTitle;
        await _offerRepository.SaveChangesAsync();

        var subject = request.UpdatedTitle == "Accepted"
            ? "Your application has been accepted"
            : "Your application has been denied";

        var body = request.UpdatedTitle == "Accepted"
            ? "Congratulations! We invite you to our department for the second stage."
            : "We regret to inform you that your application has been denied.";

        await _emailSender.SendEmailAsync(jobApplication.Candidate.Email, subject, body);

    }
}
