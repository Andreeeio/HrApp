using MediatR;
using Microsoft.AspNetCore.Http;

namespace HrApp.Application.Users.Command.ImportUsersFromExcel;

public class ImportUsersFromExcelCommand : IRequest
{
    public IFormFile ExcelFile { get; set; } = default!;
}
