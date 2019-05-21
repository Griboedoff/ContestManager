using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Contests;
using Core.Models;
using Core.Registration;
using Core.Sessions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Front.React.Controllers
{
    public class ContestsController : BaseController
    {
        private readonly IUserCookieManager cookieManager;
        private readonly IContestManager contestManager;
        private readonly IUserManager userManager;

        public ContestsController(
            IUserCookieManager cookieManager,
            IContestManager contestManager,
            IUserManager userManager)
        {
            this.cookieManager = cookieManager;
            this.contestManager = contestManager;
            this.userManager = userManager;
        }

        [HttpPost]
        public string Create(string name)
        {
            var user = cookieManager.GetUser(Request);
            var contest = contestManager.Create(name, user.Id);

            return JsonConvert.SerializeObject(contest);
        }

        [HttpGet]
        public async Task<JsonResult> List()
        {
            var contests = await contestManager.GetAll();

            return Json(
                contests
                    .OrderByDescending(c => c.CreationDate)
                    .ToArray());
        }

        [HttpGet("{id}/news")]
        public async Task<string> GetNews(Guid id)
        {
            var news = await contestManager.GetNews(id);

            return JsonConvert.SerializeObject(news);
        }

        [HttpGet("{id}/info")]
        public async Task<string> Get(Guid id)
        {
            var contest = await contestManager.Get(id);

            return JsonConvert.SerializeObject(contest);
        }

        [HttpPost("{id}/participate")]
        public void Participate(Guid id, FieldWithValue[] values)
        {
//            var user = cookieManager.GetUser(Request);
//
//            userManager.FillFields(user.Id, values);
//            contestManager.AddParticipant(id, user.Id);
        }

        [HttpGet("{id}/results")]
        public string Results(Guid id)
        {
            var participants = contestManager.GetParticipants(id);

            return JsonConvert.SerializeObject(participants);
        }
    }
}