using ClosedXML.Excel;
using DotNetEnv;
using HrApp.Application.Interfaces;
using HrApp.Application.Users.Command.AddUser;
using HrApp.Domain.Constants;
using HrApp.Domain.Entities;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Interfaces;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Users.Command.ImportUsersFromExcel;

public class ImportUsersFromExcelCommandHandler : IRequestHandler<ImportUsersFromExcelCommand>
{
    private readonly ISender _sender;
    private readonly ILogger<ImportUsersFromExcelCommandHandler> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IUserContext _userContext;
    private readonly ITeamAuthorizationService _teamAuthorizationService;
    private readonly IExellImportRepository _exellImportRepository;

    public ImportUsersFromExcelCommandHandler(ILogger<ImportUsersFromExcelCommandHandler> logger,
        ISender sender,
        IWebHostEnvironment webHostEnvironment,
        IUserContext userContext,
        ITeamAuthorizationService teamAuthorizationService,
        IExellImportRepository exellImportRepository)
    {
        _sender = sender;
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
        _userContext = userContext;
        _teamAuthorizationService = teamAuthorizationService;
        _exellImportRepository = exellImportRepository;
    }

    public async Task Handle(ImportUsersFromExcelCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Importing users from Excel file");

        if (!_teamAuthorizationService.Authorize(ResourceOperation.Create))
        {
            throw new UnauthorizedException("You do not have permission to access this resource.");
        }

        if (request.ExcelFile is null || request.ExcelFile.Length == 0)
            throw new BadRequestException("No file was uploaded.");

        var user = _userContext.GetCurrentUser();
        if (user is null)
            throw new UnauthorizedException("You must be logged in to import users from Excel.");

        var ext = Path.GetExtension(request.ExcelFile.FileName).ToLowerInvariant();
        if (ext != ".xlsx")
            throw new InvalidOperationException("Only xlsx files are allowed.");

        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid() + ext;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await request.ExcelFile.CopyToAsync(fileStream, cancellationToken);
        }

        if (!File.Exists(filePath))
            throw new BadRequestException("Wystąpił błąd podczas zapisu pliku CV. Spróbuj ponownie.");

        var path = "/uploads/exels/" + uniqueFileName;

        ExcelImport excelImport = new ExcelImport
        {
            FilePath = path,
            UploadedById = Guid.Parse(user.id),
            ImportDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        await _exellImportRepository.AddImportAsync(excelImport);

        using var stream = request.ExcelFile.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(1);

        foreach (var row in worksheet.RowsUsed().Skip(1))
        {
            var firstName = row.Cell(1).GetValue<string>();
            var lastName = row.Cell(2).GetValue<string>();
            var email = row.Cell(3).GetValue<string>();
            var dateOfBirth = DateOnly.FromDateTime(row.Cell(4).GetDateTime());
            var password = row.Cell(5).GetValue<string>();

            var command = new AddUserCommand
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                DateOfBirth = dateOfBirth
            };

            try
            {
                await _sender.Send(command, cancellationToken);
            }
            catch (BadRequestException)
            {
                continue;
            }
        }
        
    }
}