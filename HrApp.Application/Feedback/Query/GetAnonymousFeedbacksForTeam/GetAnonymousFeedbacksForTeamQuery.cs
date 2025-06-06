using HrApp.Application.Feedback.DTO;
using MediatR;

namespace HrApp.Application.Feedback.Query.GetAnonymousFeedbacksForTeam;

public class GetAnonymousFeedbacksForTeamQuery : IRequest<List<AnonymousFeedbackDTO>>
{
    public Guid TeamId { get; set; }

    public GetAnonymousFeedbacksForTeamQuery(Guid teamId)
    {
        TeamId = teamId;
    }
}
