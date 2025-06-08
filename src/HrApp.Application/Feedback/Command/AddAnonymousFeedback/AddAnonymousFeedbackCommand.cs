using HrApp.Application.Feedback.DTO;
using MediatR;

namespace HrApp.Application.Feedback.Command.AddAnonymousFeedback;

public class AddAnonymousFeedbackCommand : AnonymousFeedbackDTO, IRequest
{
}
