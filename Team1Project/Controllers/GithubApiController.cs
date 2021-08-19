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
    }
}
