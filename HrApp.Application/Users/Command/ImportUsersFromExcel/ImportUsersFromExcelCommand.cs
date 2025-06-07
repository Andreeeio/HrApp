using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Users.Command.ImportUsersFromExcel
{
    public class ImportUsersFromExcelCommand : IRequest
    {
        public IFormFile ExcelFile { get; set; } = default!;
    }
}
