using HrApp.Application.SalaryHistories.DTO;
using MediatR;

namespace HrApp.Application.SalaryHistories.Query.GetSalaryHistoryForUser;

public class GetSalaryHistoryForUserQuery : IRequest<List<SalaryHistoryDTO>>
{
    public Guid EmpId { get; set; }
    public int? HowMany { get; set; }

    public GetSalaryHistoryForUserQuery(Guid empId, int? howMany)
    {
        EmpId = empId;
        HowMany = howMany;
    }
}
