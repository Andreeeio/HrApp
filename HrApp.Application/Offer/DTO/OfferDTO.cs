namespace HrApp.Application.Offer.DTO;

public class OfferDTO
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; } 
    public string PositionName { get; set; } = default!;
    public float Salary { get; set; }
    public string Description { get; set; } = default!;
    public DateOnly AddDate { get; set; }
}