using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Team1Project.Data;
using Team1Project.Exceptions;
using Team1Project.Models;
using Team1Project.Services;

namespace Team1Project.Controllers
{
    [Authorize]

    public class InternsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GithubApiController githubApiController;
        private readonly IInternBroadcastService broadcastService;

        public InternsController(ApplicationDbContext context, IInternBroadcastService broadcastService)
        {
            _context = context;
            githubApiController = new GithubApiController(context);
            this.broadcastService = broadcastService;
        }

        // GET: Interns
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Intern.Include(i => i.Team);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Interns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intern = await _context.Intern
                .Include(i => i.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (intern == null)
            {
                return NotFound();
            }

            return View(intern);
        }

        [Authorize(Roles = "Operator")]
        // GET: Interns/Create
        public IActionResult Create()
        {
            ViewData["TeamId"] = new SelectList(_context.Set<Team>(), "Id", "Id");
            return View();
        }

        [Authorize(Roles = "Operator")]
        // POST: Interns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BirthDate,EmailAddress,TeamId,GithubUsername")] Intern intern)
        {
            if (ModelState.IsValid)
            {
                _context.Add(intern);
                await _context.SaveChangesAsync();
                broadcastService.InternAdded(intern.Id, intern.Name, intern.BirthDate, intern.EmailAddress, intern.GithubUsername, intern.TeamId);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.Set<Team>(), "Id", "Id", intern.TeamId);
            return View(intern);
        }

        [Authorize(Roles = "Operator")]
        // GET: Interns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intern = await _context.Intern.FindAsync(id);
            if (intern == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Set<Team>(), "Id", "Id", intern.TeamId);
            return View(intern);
        }

        [Authorize(Roles = "Operator")]
        // POST: Interns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BirthDate,EmailAddress,TeamId, GithubUsername")] Intern intern)
        {
            if (id != intern.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(intern);
                    await _context.SaveChangesAsync();
                    broadcastService.InternUpdated(intern.Id, intern.Name, intern.BirthDate, intern.EmailAddress, intern.GithubUsername, intern.TeamId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternExists(intern.Id))
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
            ViewData["TeamId"] = new SelectList(_context.Set<Team>(), "Id", "Id", intern.TeamId);
            return View(intern);
        }

        [Authorize(Roles = "Operator")]
        // GET: Interns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intern = await _context.Intern
                .Include(i => i.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (intern == null)
            {
                return NotFound();
            }

            return View(intern);
        }

        [Authorize(Roles = "Operator")]
        // POST: Interns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var intern = await _context.Intern.FindAsync(id);
            _context.Intern.Remove(intern);
            await _context.SaveChangesAsync();
            broadcastService.InternDeleted(intern.Id);
            return RedirectToAction(nameof(Index));
        }

        private bool InternExists(int id)
        {
            return _context.Intern.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<int> GetAge(int? id)
        {
            var intern = await _context.Intern
            .Include(i => i.Team)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (id == null)
            {
                return -1;
            }

            return intern.getAge();
        }

        public IActionResult ListRepos(int? id)
        {   

            //apicall to git
            List<string> repoLinks = githubApiController.ListRepos(id);

            return View(repoLinks);
        }

        public Task<int> GetNumberOfRepos(int? id)
        {
            return githubApiController.GetNumberOfRepos(id);
        }
    }
}
