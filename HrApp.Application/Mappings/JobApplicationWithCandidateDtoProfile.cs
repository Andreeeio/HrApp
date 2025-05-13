using AutoMapper;
using HrApp.Application.Offer.DTO;
using HrApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Mappings
{
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
}
