using AutoMapper;
using HrApp.Application.Feedback.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Feedback.DTO
{
    public class AnonymousFeedbackProfile : Profile
    {
        public AnonymousFeedbackProfile() 
        {
            CreateMap<Domain.Entities.AnonymousFeedback, AnonymousFeedbackDTO>();
            CreateMap<AnonymousFeedbackDTO, Domain.Entities.AnonymousFeedback>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<AddAnonymousFeedbackCommand, Domain.Entities.AnonymousFeedback>();
        }
    }
}
