using HrApp.Application.Users.DTO;
using MediatR;

namespace HrApp.Application.Users.Query.GetDataFromToken;

public class GetDataFromTokenQuery : IRequest<CurrentUser?>
{
}
