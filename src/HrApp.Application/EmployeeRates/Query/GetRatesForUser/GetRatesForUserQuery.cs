using HrApp.Application.EmployeeRates.DTO;
using MediatR;

namespace HrApp.Application.EmployeeRates.Query.GetRatesForUser;

public class GetRatesForUserQuery : IRequest<(List<EmployeeRateDto>,float)>
{
    public GetRatesForUserQuery(Guid userId)
    {
        UserId = userId;
    }
    public Guid UserId { get; }
}
