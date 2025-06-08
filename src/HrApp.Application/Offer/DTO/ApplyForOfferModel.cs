using Microsoft.AspNetCore.Http;

namespace HrApp.Application.Offer.DTO;

public class ApplyForOfferModel
{
    public Guid OfferId { get; set; }
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int HomeNumber { get; set; }
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public IFormFile CvFile { get; set; } = default!;
}
