namespace HrApp.Domain.Entities;

public class Candidate
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int HomeNumber { get; set; }
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public Guid ApplicationId { get; set; }
    public virtual Application Application { get; set; } = default!;
}
