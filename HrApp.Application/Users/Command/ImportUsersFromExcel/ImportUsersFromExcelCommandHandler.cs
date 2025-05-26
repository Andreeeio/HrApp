using ClosedXML.Excel;
using HrApp.Application.Users.Command.AddUser;
using HrApp.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HrApp.Application.Users.Command.ImportUsersFromExcel;

public class ImportUsersFromExcelCommandHandler : IRequestHandler<ImportUsersFromExcelCommand>
{
    private readonly ISender _sender;
    private readonly ILogger<ImportUsersFromExcelCommandHandler> _logger;

    public ImportUsersFromExcelCommandHandler(ILogger<ImportUsersFromExcelCommandHandler> logger,ISender sender)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Handle(ImportUsersFromExcelCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Importing users from Excel file");
        using var stream = request.ExcelFile.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(1);

        foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header
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
            catch (BadRequestException ex)
            {
                // logger.LogWarning("Pominięto użytkownika: {email}, powód: {message}", email, ex.Message);
                continue;
            }
        }
    }
}