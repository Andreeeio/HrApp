using AutoMapper;
using HrApp.Application.Offer.Command.CreateOffer;
using HrApp.Application.Offers.Command.CreateOffer;
using HrApp.Domain.Entities;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HrApp.Application.Offers.Command.CreateOffer
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand>
    {
        private readonly IOfferRepository _repository;
        private readonly ILogger<CreateOfferCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateOfferCommandHandler(IOfferRepository repository, ILogger<CreateOfferCommandHandler> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = _mapper.Map<HrApp.Domain.Entities.Offer>(request);
            await _repository.CreateOffer(offer);
            _logger.LogInformation($"Created offer {offer.Id} for team {offer.TeamId}");
            return;
        }
    }
}
