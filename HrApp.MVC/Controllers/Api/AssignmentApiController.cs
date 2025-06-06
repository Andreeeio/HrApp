using HrApp.Application.Assignment.Query.GetAssignmentsForApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HrApp.MVC.Controllers.Api;

[ApiController]
[Route("api/Assignments")]
public class AssignmentApiController : ControllerBase
{
    private readonly ISender _sender;

    public AssignmentApiController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignments(
            [FromQuery] string? name,
            [FromQuery] bool? isEnded,
            [FromQuery] Guid? assignedToTeamId,
            [FromQuery] int? difficultyLevel)
    {
        var query = new GetAssignmentsForApiQuery
        {
            Name = name,
            IsEnded = isEnded,
            AssignedToTeamId = assignedToTeamId,
            DifficultyLevel = difficultyLevel
        };

        var assignments = await _sender.Send(query);

        return Ok(assignments);
    }
}


