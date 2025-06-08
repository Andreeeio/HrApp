using MediatR;

namespace HrApp.Application.Salary.Query.GetByUserId;

public class GetPaidByUserIdQuery : IRequest<Domain.Entities.Paid>
{
    public Guid Id { get; }

    public GetPaidByUserIdQuery(Guid id)
    {
        Id = id;
    }
}
