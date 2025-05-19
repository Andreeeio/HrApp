using AutoMapper;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Feedback.Command
{
    public class AddAnonymousFeedbackCommandHandler : IRequestHandler<AddAnonymousFeedbackCommand>
    {
        private readonly IFeedbackRepository _repository;
        private readonly ILogger<AddAnonymousFeedbackCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddAnonymousFeedbackCommandHandler(ILogger<AddAnonymousFeedbackCommandHandler> logger, IFeedbackRepository repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Handle(AddAnonymousFeedbackCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding anonymous feedback with subject: {Subject} and message: {Message}", request.Subject, request.Message);
            request.CreatedAt = DateTime.UtcNow;
            var feedback = _mapper.Map<AnonymousFeedback>(request);
            await _repository.AddAnonymousFeedback(feedback);
            return;
        }
    }

}
