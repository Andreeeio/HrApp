using AutoMapper;
using HrApp.Application.Offer.Command.CreateJobApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Mappings
{
    public class CreateJobApplicationCommandProfile : Profile
    {
        public CreateJobApplicationCommandProfile()
        {
            CreateMap<HrApp.Domain.Entities.JobApplication, CreateJobApplicationCommand>();
            CreateMap<CreateJobApplicationCommand, HrApp.Domain.Entities.JobApplication>();
        }
    }
}
