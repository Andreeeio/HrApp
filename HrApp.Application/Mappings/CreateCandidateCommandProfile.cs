using AutoMapper;
using HrApp.Application.Offer.Command.CreateCandidate;
using HrApp.Application.Offer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Mappings
{
    public class CreateCandidateCommandProfile : Profile
    {
        public CreateCandidateCommandProfile()
        {
            CreateMap<HrApp.Domain.Entities.Candidate, CreateCandidateCommand>();
            CreateMap<CreateCandidateCommand, HrApp.Domain.Entities.Candidate>();
        }
    }
}
