using AutoMapper;
using HrApp.Application.Feedback.Command.AddAnonymousFeedback;

namespace HrApp.Application.Feedback.DTO;

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
