namespace HrApp.Application.Offer.DTO;

public class JobApplicationDTO
{
    public Guid Id { get; set; }
    public Guid OfferID { get; set; }
    public Guid CandidateId { get; set; }
    public DateOnly ApplicationDate { get; set; }
    public string Status { get; set; } = default!;
    public string CvLink { get; set; } = default!;
}
