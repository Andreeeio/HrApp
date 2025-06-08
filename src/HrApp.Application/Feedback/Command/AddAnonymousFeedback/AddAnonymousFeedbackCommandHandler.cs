using AutoMapper;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Feedback.Command.AddAnonymousFeedback;

public class AddAnonymousFeedbackCommandHandler : IRequestHandler<AddAnonymousFeedbackCommand>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly ILogger<AddAnonymousFeedbackCommandHandler> _logger;
    private readonly IMapper _mapper;

    public AddAnonymousFeedbackCommandHandler(ILogger<AddAnonymousFeedbackCommandHandler> logger, IFeedbackRepository feedbackRepository, IMapper mapper)
    {
        _feedbackRepository = feedbackRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Handle(AddAnonymousFeedbackCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding anonymous feedback with subject: {Subject} and message: {Message}", request.Subject, request.Message);
        request.CreatedAt = DateTime.UtcNow;
        var feedback = _mapper.Map<AnonymousFeedback>(request);
        await _feedbackRepository.AddAnonymousFeedbackAsync(feedback);
    }
}
