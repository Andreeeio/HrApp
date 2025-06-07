using System.Text;

namespace HrApp.Domain.Entities;

public class UserRaport
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public float YearRoundSalary { get; set; }
    public Guid? TeamId { get; set; }
    public Guid OverallRaportId { get; set; }
    public virtual OverallRaport OverallRaport { get; set; } = default!;
}
