using AutoMapper;
using HrApp.Application.Feedback.DTO;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Feedback.Query
{
    public class GetAnonymousFeedbacksForTeamQueryHandler : IRequestHandler<GetAnonymousFeedbacksForTeamQuery, List<AnonymousFeedbackDTO>>
    {
        private readonly IFeedbackRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAnonymousFeedbacksForTeamQueryHandler> _logger;

        public GetAnonymousFeedbacksForTeamQueryHandler(ILogger<GetAnonymousFeedbacksForTeamQueryHandler> logger,IFeedbackRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<AnonymousFeedbackDTO>> Handle(GetAnonymousFeedbacksForTeamQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting anonymous feedbacks for team with ID: {TeamId}", request.TeamId);
            var feedbacks = await _repository.GetAnonymousFeedbacksForTeam(request.TeamId);
            return _mapper.Map<List<AnonymousFeedbackDTO>>(feedbacks);
        }
    }

}
