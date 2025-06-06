using HrApp.Domain.Entities;
using MediatR;

namespace HrApp.Application.Salary.Query.GetByUserId;

public class GetPaidByUserIdQuery : IRequest<Paid>
{
    public Guid Id { get; }

    public GetPaidByUserIdQuery(Guid id)
    {
        Id = id;
    }
}
