using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Offer.Command.CreateCandidate
{
    public class CreateCandidateCommand : IRequest<Guid>
    {
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int HomeNumber { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
    }
}
