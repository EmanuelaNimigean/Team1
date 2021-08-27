using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Team1Project.Data;
using Team1Project.Models;
using Team1Project.Services;

namespace Team1Project.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITeamBroadcastService broadcastService;

        public TeamsController(ApplicationDbContext context, ITeamBroadcastService broadcastService)
        {
            _context = context;
            this.broadcastService= broadcastService;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Team.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            // obtain interns 
            List<Intern> interns = await _context.Intern.Where(i => i.TeamId == team.Id).ToListAsync();
            team.Interns = interns;

            return View(team);
        }

        [Authorize(Roles = "Operator")]
        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Operator")]
        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JiraBoardUrl,Git,Emblem,Motto")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                broadcastService.NewTeamAdded(team.Id, team.JiraBoardUrl, team.Git, team.Emblem, team.Motto);
                
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        [Authorize(Roles = "Operator")]
        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        [Authorize(Roles = "Operator")]
        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JiraBoardUrl,Git,Emblem,Motto")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                    broadcastService.TeamUpdated(team.Id, team.JiraBoardUrl, team.Git, team.Emblem, team.Motto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        [Authorize(Roles = "Operator")]
        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [Authorize(Roles = "Operator")]
        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Team.FindAsync(id);
            _context.Team.Remove(team);
            await _context.SaveChangesAsync();
            broadcastService.TeamDeleted(team.Id);

            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        private bool TeamExists(int id)
        {
            return _context.Team.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetInfo(int teamId)
        {
            var team = await _context.Team
               .FirstOrDefaultAsync(m => m.Id == teamId);

            if (team == null)
            {
                return NotFound();
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<double> GetAverageAge(int teamId)
        {
            //https://www.brentozar.com/archive/2016/09/select-specific-columns-entity-framework-query/
            var interns = _context.Intern.Where(i => i.TeamId == teamId);

            if (interns.Count() == 0)
            {
                return 0.0;
            }

            double avgAge = 0.0;
            foreach (Intern intern in interns)
            {
                avgAge += intern.getAge();
            }
            avgAge /= interns.Count();


            return avgAge;
        }

        [HttpGet]
        public async Task<List<Intern>> GetInterns(int teamId)
        {
            /*var interns = _context.Intern.Where(i => i.TeamId == teamId);
*/
            var team = await _context.Team
               .FirstOrDefaultAsync(m => m.Id == teamId); 
            if (team == null)
            {
                return null;
            }

            return team.Interns;
        }
    }
}
