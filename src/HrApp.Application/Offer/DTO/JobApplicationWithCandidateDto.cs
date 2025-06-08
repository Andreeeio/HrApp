namespace HrApp.Application.Offer.DTO;

public class JobApplicationWithCandidateDto
{
    public DateOnly ApplicationDate { get; set; }
    public string Status { get; set; } = default!;
    public string CvLink { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string City { get; set; } = default!;
    public Guid Id { get; set; }
}
