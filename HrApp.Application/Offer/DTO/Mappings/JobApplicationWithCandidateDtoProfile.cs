using AutoMapper;
using HrApp.Domain.Entities;

namespace HrApp.Application.Offer.DTO.Mappings;

public class JobApplicationWithCandidateDtoProfile : Profile
{
    public JobApplicationWithCandidateDtoProfile()
    {
        CreateMap<JobApplication, JobApplicationWithCandidateDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Candidate.Name))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Candidate.Surname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Candidate.Email))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Candidate.City));
    }
}
