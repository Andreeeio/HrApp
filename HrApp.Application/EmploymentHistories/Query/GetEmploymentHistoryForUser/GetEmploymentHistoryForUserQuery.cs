using HrApp.Application.EmploymentHistories.DTO;
using MediatR;

namespace HrApp.Application.EmploymentHistories.Query.GetEmploymentHistoryForUser;

public class GetEmploymentHistoryForUserQuery : IRequest<List<EmploymentHistoryDTO>>
{
    public GetEmploymentHistoryForUserQuery(Guid userId)
    {
        UserId = userId;
    }
    public Guid UserId { get; }
}
