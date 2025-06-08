using AutoMapper;
using HrApp.Application.Offer.Command.CreateCandidate;

namespace HrApp.Application.Offer.DTO.Mappings;

public class CreateCandidateCommandProfile : Profile
{
    public CreateCandidateCommandProfile()
    {
        CreateMap<Domain.Entities.Candidate, CreateCandidateCommand>();
        
        CreateMap<CreateCandidateCommand, Domain.Entities.Candidate>();
    }
}
