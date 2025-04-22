namespace HrApp.Domain.Entities;

public class Paid
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public decimal BaseSalary { get; set; }
    public EmployeeRate EmployeeRate { get; set; } = default!;
}
