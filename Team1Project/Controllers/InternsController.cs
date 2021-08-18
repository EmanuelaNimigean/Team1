using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Team1Project.Data;
using Team1Project.Models;

namespace Team1Project.Controllers
{
    public class InternsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InternsController(ApplicationDbContext context)
        {
            _context = context;
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

        // GET: Interns/Create
        public IActionResult Create()
        {
            ViewData["TeamId"] = new SelectList(_context.Set<Team>(), "Id", "Id");
            return View();
        }

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
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.Set<Team>(), "Id", "Id", intern.TeamId);
            return View(intern);
        }

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

        // POST: Interns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var intern = await _context.Intern.FindAsync(id);
            _context.Intern.Remove(intern);
            await _context.SaveChangesAsync();
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

        [HttpGet]
        public async Task<int> GetNumberOfRepos(int? id)
        {
            var intern = await _context.Intern
            .Include(i => i.Team)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (id == null)
            {
                return -1;
            }
            return GetPublicRepositories(intern.GithubUsername).Count;
        }

        public async Task<IActionResult> ListRepos(int? id)
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

            var username = intern.GithubUsername;

            //apicall to git
            List<string> repoLinks = GetPublicRepositories(username);
            

            return View(repoLinks);
        }

        private List<string> GetPublicRepositories(string username)
        {
            var client = new RestClient($"https://api.github.com/users/{username}/repos");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            return ConvertResponseToRepositoriesList(response.Content);
        }

        [NonAction]
        public List<string> ConvertResponseToRepositoriesList(string content)
        {
            List<string> repoLinks = new List<string>();
            /*var json = JObject.Parse(content);*/
            var json = JsonConvert.DeserializeObject<List<JObject>>(content);

            foreach (var repo in json)
            {
                if (repo["full_name"] == null)
                {
                    throw new Exception("Username not valid.");
                }


                repoLinks.Add($"https://github.com/{repo["full_name"]}");
            }

            return repoLinks;
        }
    }
}
