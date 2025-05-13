using AutoMapper;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Offer.Command.CreateJobApplication
{
    public class CreateJobApplicationCommandHandler : IRequestHandler<CreateJobApplicationCommand>
    {
        private readonly ILogger<CreateJobApplicationCommandHandler> _logger;
        private readonly IOfferRepository _repository;
        private readonly IMapper _mapper;
        public CreateJobApplicationCommandHandler(ILogger<CreateJobApplicationCommandHandler> logger, IOfferRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating job application for offer with ID: {OfferID} and candidate with ID: {CandidateId}", request.OfferId, request.CandidateId);
            var jobApplication = _mapper.Map<Domain.Entities.JobApplication>(request);
            await _repository.CreateJobApplication(jobApplication);
            return;
        }
    }
}
