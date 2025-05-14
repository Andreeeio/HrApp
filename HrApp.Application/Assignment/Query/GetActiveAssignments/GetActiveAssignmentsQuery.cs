using HrApp.Application.Assignment.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Assignment.Query.GetActiveAssignments
{
    public class GetActiveAssignmentsQuery : IRequest<List<AssignmentDTO>>
    {
    }
}
