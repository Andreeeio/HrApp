using AutoMapper;
using HrApp.Application.Feedback.DTO;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Feedback.Query.GetAnonymousFeedbacksForTeam;

public class GetAnonymousFeedbacksForTeamQueryHandler : IRequestHandler<GetAnonymousFeedbacksForTeamQuery, List<AnonymousFeedbackDTO>>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAnonymousFeedbacksForTeamQueryHandler> _logger;

    public GetAnonymousFeedbacksForTeamQueryHandler(ILogger<GetAnonymousFeedbacksForTeamQueryHandler> logger,IFeedbackRepository feedbackRepository, IMapper mapper)
    {
        _feedbackRepository = feedbackRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<AnonymousFeedbackDTO>> Handle(GetAnonymousFeedbacksForTeamQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting anonymous feedbacks for team with ID: {TeamId}", request.TeamId);
        var feedbacks = await _feedbackRepository.GetAnonymousFeedbacksForTeamAsync(request.TeamId);
        return _mapper.Map<List<AnonymousFeedbackDTO>>(feedbacks);
    }
}
