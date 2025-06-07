using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;

namespace HrApp.Application.Assignment.Command.AddAssignmentToTeam;

public  class AddAssignmentToTeamCommandHandler : IRequestHandler<AddAssignmentToTeamCommand>
{
    private readonly IAssignmentRepository _repository;

    public AddAssignmentToTeamCommandHandler(IAssignmentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(AddAssignmentToTeamCommand request, CancellationToken cancellationToken)
    {
        var assignment = await _repository.GetAssignmentByIdAsync(request.AssignmentId);

        if (assignment == null) 
            throw new BadRequestException($"Assignment with ID {request.AssignmentId} not found.");

        assignment.AssignedToTeamId = request.TeamId;

        await _repository.SaveChangesAsync();
    }
}
