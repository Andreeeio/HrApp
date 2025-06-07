using MediatR;

namespace HrApp.Application.Offer.Command.UpdateJobApOffer;

public class UpdateJobApOfferCommand : IRequest
{
    public Guid JobApId { get; set; }
    public string UpdatedTitle { get; set; } = string.Empty;

    public UpdateJobApOfferCommand() { }

    public UpdateJobApOfferCommand(Guid JobApIp)
    {
        JobApId = JobApIp;
    }
}
