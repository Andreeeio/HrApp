using AutoMapper;
using Google.Apis.Calendar.v3.Data;
using HrApp.Application.Calendars.Command.CreateCalendarEvent;

namespace HrApp.Application.Calendars.DTO;

public class CalendarProfile : Profile
{
    public CalendarProfile()
    {
        CreateMap<Event, CalendarDTO>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Summary))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDateTimeOffset!.Value.DateTime))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Start.DateTimeDateTimeOffset!.Value.DateTime))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.End.DateTimeDateTimeOffset!.Value.DateTime))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        CreateMap<CreateCalendarEventCommand, Domain.Entities.Calendar>();
    }
}
