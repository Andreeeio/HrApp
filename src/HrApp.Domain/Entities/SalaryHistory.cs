namespace HrApp.Domain.Entities;

public class SalaryHistory
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public float Salary { get; set; }
    public DateOnly MonthNYear { get; set; }
}
