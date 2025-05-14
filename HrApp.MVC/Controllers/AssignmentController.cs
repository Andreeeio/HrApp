using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediatR;
using HrApp.Application.Assignment.Query.GetAssignmentForTeam;
using HrApp.Application.Teams.Query.GetAllTeams;
using HrApp.Application.Assignment.Command.AddAssignment;
using HrApp.Application.Teams.Query.GetTeamForUser;
using HrApp.Application.Users.Query.GetDataFromToken;
using HrApp.Domain.Entities;
using HrApp.Application.Assignment.Query.GetActiveAssignments;
using Microsoft.EntityFrameworkCore;
using HrApp.Application.Assignment.Query.GetFreeAssignments;
using HrApp.Application.Assignment.Command.AddAssignmentToTeam;

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

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var teams = await _sender.Send(new GetAllTeamsQuery());
        //ViewBag.teams = new SelectList(teams, "Id", "Name");
        var teamList = teams.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name
        }).ToList();

        // Dodaj opcję pustą (null)
        teamList.Insert(0, new SelectListItem
        {
            Value = "", // pusty string = null po stronie modelu
            Text = "-- Brak zespołu --",
            Selected = true
        });

        ViewBag.teams = teamList;
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
        if (team == null)
        {
            return RedirectToAction("ShowFreeAssignments");
        }
        return RedirectToAction("EmployersInTeam", "Team", new { TeamId = team.Id, TeamName = team.Name });
    }

    [HttpGet("ShowFreeAssignments")]
    public async Task<IActionResult> ShowFreeAssignments()
    {
        var assignments = await _sender.Send(new GetFreeAssignmentsQuery());
        return View("Index", assignments);
    }

    [HttpGet("AddAssignmentToTeam/{id}")]
    public async Task<IActionResult> AddAssignmentToTeam(Guid id)
    {
        var teams = await _sender.Send(new GetAllTeamsQuery());
        ViewBag.teams = teams
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            })
            .ToList();
        ViewBag.AssignmentId = id;
        return View();
    }
    [HttpPost("AddAssignmentToTeam/{id}")]
    public async Task<IActionResult> AddAssignmentToTeam(AddAssignmentToTeamCommand command)
    {
        if (!ModelState.IsValid)
        {
            // Jeśli potrzebujesz znowu listy zespołów (np. po błędzie), załaduj ją ponownie
            var teams = await _sender.Send(new GetAllTeamsQuery());
            ViewBag.teams = teams
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                })
                .ToList();
            ViewBag.AssignmentId = command.AssignmentId;
            return View(command);
        }

        await _sender.Send(command);
        return RedirectToAction("Index", "Departments");
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
