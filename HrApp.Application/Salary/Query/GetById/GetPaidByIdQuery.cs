using HrApp.Domain.Entities;
using MediatR;

namespace HrApp.Application.Salary.Query.GetById;

public class GetPaidByIdQuery(Guid id) : IRequest<Paid>
{
    public Guid Id { get; set; } = id;
}
