namespace HrApp.Domain.Entities;

public class TeamRaport
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Guid TeamLeaderId { get; set; }
    public string Name { get; set; } = default!;
    public Guid OverallRaportId { get; set; }
    public virtual OverallRaport OverallRaport { get; set; } = default!;
}