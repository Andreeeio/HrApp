using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using HrApp.Application.Calendars.DTO;
using HrApp.Application.Interfaces;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Calendars.Query.GetGoogleCalendarEvents;

public class GetGoogleCalendarEventsQueryHandler(ILogger<GetGoogleCalendarEventsQueryHandler> logger,
    ICalendarService googleAuthService,
    IMapper mapper) : IRequestHandler<GetGoogleCalendarEventsQuery, List<CalendarDTO>>
{
    private readonly ILogger<GetGoogleCalendarEventsQueryHandler> _logger = logger;
    private readonly ICalendarService _googleAuthService = googleAuthService;
    private readonly IMapper _mapper = mapper;
    private readonly string CalendarId = googleAuthService.CalendarId;
    public async Task<List<CalendarDTO>> Handle(GetGoogleCalendarEventsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetGoogleCalendarEventsQuery for user");

        var service = _googleAuthService.GetCalendarService();
        if (service == null) 
            throw new Exception("Google Calendar service is not initialized. Ensure the service account JSON file is correctly configured.");
        
        var listReq = service.Events.List(CalendarId);
        listReq.TimeMinDateTimeOffset = DateTimeOffset.UtcNow;
        listReq.ShowDeleted = false;
        listReq.SingleEvents = true;
        listReq.MaxResults = 100;
        listReq.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        var response = await listReq.ExecuteAsync(cancellationToken);
        List<Event> events = response.Items.ToList();
        var dto = _mapper.Map<List<CalendarDTO>>(events);
        
        return dto;
    }
}
