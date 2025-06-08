using AutoMapper;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Offer.Command.CreateCandidate;

public class CreateCandidateCommandHandler : IRequestHandler<CreateCandidateCommand, Guid>
{
    private readonly ILogger<CreateCandidateCommandHandler> _logger;
    private readonly IOfferRepository _repository;
    private readonly IMapper _mapper;
    public CreateCandidateCommandHandler(ILogger<CreateCandidateCommandHandler> logger, IOfferRepository repository, IMapper mapper) 
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<Guid> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating candidate with name: {Name} and surname: {Surname}", request.Name, request.Surname);
        var candidate = _mapper.Map<Domain.Entities.Candidate>(request);
        await _repository.CreateCandidateAsync(candidate);

        return await Task.FromResult(candidate.Id);
    }
}
