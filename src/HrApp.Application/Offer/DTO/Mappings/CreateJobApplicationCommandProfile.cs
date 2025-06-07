using AutoMapper;
using HrApp.Application.Offer.Command.CreateJobApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Offer.DTO.Mappings
{
    public class CreateJobApplicationCommandProfile : Profile
    {
        public CreateJobApplicationCommandProfile()
        {
            CreateMap<Domain.Entities.JobApplication, CreateJobApplicationCommand>();
            CreateMap<CreateJobApplicationCommand, Domain.Entities.JobApplication>();
        }
    }
}
