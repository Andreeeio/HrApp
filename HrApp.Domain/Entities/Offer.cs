namespace HrApp.Domain.Entities;

public class Offer
{
    public Guid Id { get; set; }
    public string PositionName { get; set; } = default!;
    public float Salary { get; set; }
    public string Descritpion { get; set; } = default!;
    public DateOnly AddDate { get; set; }
    public List<Application> Applications { get; set; } = default!;
}
