using HrApp.Application.Users.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Users.Query.GetUserByEmail
{
    public class GetUserByEmailQuery : IRequest<UserDTO>
    {
        public string Email { get; set; } = string.Empty;
        public GetUserByEmailQuery(string email)
        {
            Email = email;
        }
    }
}
