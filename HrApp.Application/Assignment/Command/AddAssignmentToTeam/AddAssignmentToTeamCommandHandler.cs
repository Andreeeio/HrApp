using HrApp.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Assignment.Command.AddAssignmentToTeam
{
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

            assignment.AssignedToTeamId = request.TeamId;

            await _repository.UpdateAsync(assignment);

            return;
        }
    }


}
