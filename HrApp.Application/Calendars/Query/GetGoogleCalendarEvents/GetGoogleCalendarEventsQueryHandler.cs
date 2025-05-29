using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using HrApp.Application.Calendars.DTO;
using HrApp.Application.Interfaces;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Calendars.Query.GetGoogleCalendarEvents;

public class GetGoogleCalendarEventsQueryHandler(ILogger<GetGoogleCalendarEventsQueryHandler> logger,
    IUserContext userContext,
    IGoogleOAuthTokenRepository googleOAuthTokenRepository,
    IMapper mapper) : IRequestHandler<GetGoogleCalendarEventsQuery, List<CalendarDTO>>
{
    private readonly ILogger<GetGoogleCalendarEventsQueryHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    private readonly IGoogleOAuthTokenRepository _googleOAuthTokenRepository = googleOAuthTokenRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<List<CalendarDTO>> Handle(GetGoogleCalendarEventsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetGoogleCalendarEventsQuery for user");

        var user = _userContext.GetCurrentUser();
        if (user == null)
            throw new UnauthorizedException("User not found in context");

        var token = await _googleOAuthTokenRepository.GetTokenByUserIdAsync(Guid.Parse(user.id));
        if (token == null || token.Expiry < DateTime.UtcNow)
            return null;

        var credential = GoogleCredential.FromAccessToken(token.AccessToken);
        var service = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "HrApp"
        });

        var listReq = service.Events.List("primary");
        listReq.TimeMinDateTimeOffset = DateTimeOffset.UtcNow;
        listReq.ShowDeleted = false;
        listReq.SingleEvents = true;
        listReq.MaxResults = 10;
        listReq.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        var response = await listReq.ExecuteAsync(cancellationToken);
        var dto = _mapper.Map<List<CalendarDTO>>(response.Items);
        return dto;
    }
}
