using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team1Project.Data;

namespace Team1Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GithubApiController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GithubApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetNumberOfRepos/{id}")]
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

        [HttpGet("ListRepos/{id}")]
        public List<string> ListRepos(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var intern = _context.Intern
               .Include(i => i.Team)
               .FirstOrDefault(m => m.Id == id);

            if (intern == null)
            {
                return null;
            }

            var username = intern.GithubUsername;

            //apicall to git
            List<string> repoLinks = GetPublicRepositories(username);


            return repoLinks;
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

        [HttpGet("GetUserCommits/{username}")]
        public int GetUserCommits(string username)
        {
            int pageIndex = 1;
            int responseSize = 0;
            int numberOfCommitsThisYear = 0;

            var client = new RestClient($"https://api.github.com/users/{username}/events");
            var request = new RestRequest(Method.GET);
            request.AddParameter("per_page", 100);

            client.Timeout = -1;

            do
            {
                request.AddParameter("page", pageIndex);
                IRestResponse response = client.Execute(request);

                var json = JsonConvert.DeserializeObject<List<JObject>>(response.Content);
                responseSize = json.Count();

                numberOfCommitsThisYear += CountCommitsFromEvents(json);

                pageIndex++;

            } while (responseSize > 0);

            return numberOfCommitsThisYear;
        }

        [NonAction]
        public int CountCommitsFromEvents(List<JObject> events)
        {
            int noCommits = 0;

            foreach (var _event in events)
            {
                if (_event["type"].ToString().Equals("PushEvent") && happendThisYear(_event["created_at"].ToString()))
                {
                    noCommits += _event["payload"]["commits"].Count();
                }

            }

            return noCommits;
        }

        [NonAction]
        public bool happendThisYear(string eventDate)
        {
            // takes "2021" from "2021-08-19T10:23:10Z"
            int eventYear = DateTime.Parse(eventDate).Year;
            int month = DateTime.Parse(eventDate).Month;

            return DateTime.Now.Year.Equals(eventYear) && DateTime.Now.Month.Equals(month);

        }

    }
}
