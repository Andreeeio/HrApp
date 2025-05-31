using AutoMapper;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using HrApp.Application.Interfaces;
using HrApp.Application.Users.DTO;
using HrApp.Domain.Constants;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HrApp.Application.Calendars.Command.CreateCalendarEvent;

public class CreateCalendarEventCommandHandler(ILogger<CreateCalendarEventCommandHandler> logger,
    IUserContext userContext,
    IGoogleOAuthTokenRepository googleOAuthTokenRepository,
    ITeamAuthorizationService teamAuthorizationService,
    ICalendarRepository calendarRepository,
    IMapper mapper,
    IConfiguration configuration) : IRequestHandler<CreateCalendarEventCommand>
{
    private readonly ILogger<CreateCalendarEventCommandHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;
    private readonly IGoogleOAuthTokenRepository _googleOAuthTokenRepository = googleOAuthTokenRepository;
    private readonly ITeamAuthorizationService _teamAuthorizationService = teamAuthorizationService;
    private readonly ICalendarRepository _calendarRepository = calendarRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IConfiguration _configuration = configuration;
    public async Task Handle(CreateCalendarEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateCalendarEventCommand for user");
        var calendarId = _configuration["GoogleAPI:calendar_id"];
        if (string.IsNullOrEmpty(calendarId))
            throw new ArgumentNullException("Google Calendar ID is not configured");

        var user = _userContext.GetCurrentUser();
        if (user == null)
            throw new UnauthorizedException("User is not authenticated");

        if (!_teamAuthorizationService.Authorize(ResourceOperation.Update))
            throw new UnauthorizedException("User is not authorized to create calendar events");

        var token = await _googleOAuthTokenRepository.GetTokenByUserIdAsync(Guid.Parse(user.id));
        if (token == null || token.Expiry < DateTime.UtcNow)
            throw new NotFoundAuthOTokenException("Not found Google OAuth token for user or token expired");

        var credential = GoogleCredential
            .FromAccessToken(token.AccessToken);

        var service = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "TeamCalendarApp"
        });

        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");

        var calendarEvent = new Event
        {
            Summary = request.Title,
            Description = request.Description,
            Start = new EventDateTime
            {
                DateTimeDateTimeOffset = new DateTimeOffset(request.StartDate, timeZone.GetUtcOffset(request.StartDate)),
                TimeZone = "Europe/Warsaw"
            },
            End = new EventDateTime
            {
                DateTimeDateTimeOffset = new DateTimeOffset(request.EndDate, timeZone.GetUtcOffset(request.EndDate)),
                TimeZone = "Europe/Warsaw"
            }
        };

        var calendar = _mapper.Map<Domain.Entities.Calendar>(request);
        calendar.CreatedDate = DateTime.UtcNow;
        calendar.Creator = new CalendarEventCreator()
        {
            CalendarId = calendar.Id,
            Email = user.email
        };

        await _calendarRepository.AddCalendarEvent(calendar);
        Console.WriteLine(calendarId);
        await service.Events.Insert(calendarEvent, calendarId).ExecuteAsync(cancellationToken);  
    }
}
