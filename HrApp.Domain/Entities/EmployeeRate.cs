using System.Runtime.CompilerServices;

namespace HrApp.Domain.Entities;

public class EmployeeRate
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public virtual User Employee { get; set; } = default!;
    public Guid RatedById { get; set; }
    public virtual User RatedBy { get; set; } = default!;
    public DateOnly RateDate { get; set; }
    public int Rate { get; set; }
}
