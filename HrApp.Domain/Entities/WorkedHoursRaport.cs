namespace HrApp.Domain.Entities;

public class WorkedHoursRaport
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public int WorkedHours { get; set; }
    public DateOnly MonthNYear { get; set; }
}