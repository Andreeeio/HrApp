namespace HrApp.Domain.Entities;

public class Paid
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public float BaseSalary { get; set; }
}
