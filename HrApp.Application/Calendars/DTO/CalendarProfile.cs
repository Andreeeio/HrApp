using AutoMapper;
using Google.Apis.Calendar.v3.Data;

namespace HrApp.Application.Calendars.DTO;

public class CalendarProfile : Profile
{
    public CalendarProfile()
    {
        CreateMap<Event, CalendarDTO>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Summary))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDateTimeOffset))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Start.DateTimeDateTimeOffset))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.End.DateTimeDateTimeOffset))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}
