using System.Runtime.CompilerServices;

namespace HrApp.Domain.Entities;

public class EmployeeRate
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public DateOnly RateDate { get; set; }
    public int Rate { get; set; }
    public List<Paid> Paids { get; set; } = default!;
}
