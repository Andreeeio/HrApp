using MediatR;

namespace HrApp.Application.Salary.Query.GetById;

public class GetPaidByIdQuery(Guid id) : IRequest<Domain.Entities.Paid>
{
    public Guid Id { get; set; } = id;
}
