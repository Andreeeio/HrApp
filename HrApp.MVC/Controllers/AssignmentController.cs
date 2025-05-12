using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediatR;
using HrApp.Application.Assignment.Query.GetAssignmentForTeam;
using HrApp.Application.Teams.Query.GetAllTeams;
using HrApp.Application.Assignment.Command.AddAssignment;
using HrApp.Application.Teams.Query.GetTeamForUser;
using HrApp.Application.Users.Query.GetDataFromToken;

namespace HrApp.MVC.Controllers;

public class AssignmentController : Controller
{
    public readonly ISender _sender;

    public AssignmentController(ISender sender)
    {
        _sender = sender;
    }

    // GET: Assignment
    [HttpGet("{TeamId}/Assignment")]
    public async Task<IActionResult> Index(Guid TeamId)
    {
        var assignments = await _sender.Send(new GetAssignmentForTeamQuery(TeamId));
        ViewBag.TeamId = TeamId;
        return View(assignments);
    }

    //// GET: Assignment/Details/5
    //public async Task<IActionResult> Details(Guid? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var assignment = await _context.Assignment
    //        .Include(a => a.AssignedToTeam)
    //        .FirstOrDefaultAsync(m => m.Id == id);
    //    if (assignment == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(assignment);
    //}

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var teams = await _sender.Send(new GetAllTeamsQuery());
        ViewBag.teams = new SelectList(teams, "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddAssignmentCommand command)
    {
        if (!ModelState.IsValid)
        {
            // Handle the command to create an assignment
            return View(command);
        }
        await _sender.Send(command);
        var user = await _sender.Send(new GetDataFromTokenQuery());
        var team = await _sender.Send(new GetTeamForUserQuery(Guid.Parse(user.id)));

        return RedirectToAction("EmployersInTeam","Team", new { TeamId = team.Id, TeamName = team.Name });
    }



    //// GET: Assignment/Edit/5
    //public async Task<IActionResult> Edit(Guid? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var assignment = await _context.Assignment.FindAsync(id);
    //    if (assignment == null)
    //    {
    //        return NotFound();
    //    }
    //    ViewData["AssignedToTeamId"] = new SelectList(_context.Team, "Id", "Name", assignment.AssignedToTeamId);
    //    return View(assignment);
    //}

    //// POST: Assignment/Edit/5
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,StartDate,EndDate,AssignedToTeamId,DifficultyLevel")] Assignment assignment)
    //{
    //    if (id != assignment.Id)
    //    {
    //        return NotFound();
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            _context.Update(assignment);
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!AssignmentExists(assignment.Id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }
    //        return RedirectToAction(nameof(Index));
    //    }
    //    ViewData["AssignedToTeamId"] = new SelectList(_context.Team, "Id", "Name", assignment.AssignedToTeamId);
    //    return View(assignment);
    //}

    //// GET: Assignment/Delete/5
    //public async Task<IActionResult> Delete(Guid? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var assignment = await _context.Assignment
    //        .Include(a => a.AssignedToTeam)
    //        .FirstOrDefaultAsync(m => m.Id == id);
    //    if (assignment == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(assignment);
    //}

    //// POST: Assignment/Delete/5
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteConfirmed(Guid id)
    //{
    //    var assignment = await _context.Assignment.FindAsync(id);
    //    if (assignment != null)
    //    {
    //        _context.Assignment.Remove(assignment);
    //    }

    //    await _context.SaveChangesAsync();
    //    return RedirectToAction(nameof(Index));
    //}

    //private bool AssignmentExists(Guid id)
    //{
    //    return _context.Assignment.Any(e => e.Id == id);
    //}
}
