using AutoMapper;
using HrApp.Application.Offer.Command.CreateCandidate;
using HrApp.Application.Offer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Offer.DTO.Mappings
{
    public class CreateCandidateCommandProfile : Profile
    {
        public CreateCandidateCommandProfile()
        {
            CreateMap<Domain.Entities.Candidate, CreateCandidateCommand>();
            CreateMap<CreateCandidateCommand, Domain.Entities.Candidate>();
        }
    }
}
