namespace HrApp.Domain.Entities;

public class JobApplication
{
    public Guid Id { get; set; }
    public Guid OfferID { get; set; }
    public virtual Offer Offer { get; set; } = default!;
    public Guid CandidateId { get; set; }
    public virtual Candidate Candidate { get; set; } = default!;
    public DateOnly ApplicationDate { get; set; }
    public string CvLink { get; set; } = default!;
}
