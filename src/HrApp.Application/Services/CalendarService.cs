using DotNetEnv;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using HrApp.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HrApp.Application.Services;

public class CalendarService : ICalendarService
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;
    private Google.Apis.Calendar.v3.CalendarService? _calendarService;

    public string CalendarId { get; set; }


    public CalendarService(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
        CalendarId = _configuration["GoogleAPI:calendar_id"] ?? throw new ArgumentNullException("GoogleAPI:calendar_id not configured");

        InitializeCalendarService();
    }

    private void InitializeCalendarService()
    {

        var credentialsPath = Path.Combine(_env.ContentRootPath, "google-service-account.json");

        GoogleCredential credential;
        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped(Google.Apis.Calendar.v3.CalendarService.Scope.Calendar);
        }

        _calendarService = new Google.Apis.Calendar.v3.CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "TeamCalendarApp"
        });
    }

    public Google.Apis.Calendar.v3.CalendarService GetCalendarService()
    {
        if (_calendarService == null)
        {
            InitializeCalendarService();
        }
        return _calendarService!;
    }
}