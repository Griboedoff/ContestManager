using System;
using System.Linq;
using System.Web.Mvc;
using Core.Managers;
using Core.Models;
using Newtonsoft.Json;

namespace Front.Controllers.api
{
    [RoutePrefix("contests")]
    public class ContestsController : Controller
    {
        private readonly ICookieManager cookieManager;
        private readonly IContestManager contestManager;
        private readonly IUserManager userManager;

        public ContestsController(ICookieManager cookieManager, IContestManager contestManager,
            IUserManager userManager)
        {
            this.cookieManager = cookieManager;
            this.contestManager = contestManager;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("")]
        public string Create(string name, string fields)
        {
            var user = cookieManager.GetUser(Request);
            var contest = contestManager.Create(
                name,
                user.Id,
                JsonConvert.DeserializeObject<FieldDescription[]>(fields));

            return JsonConvert.SerializeObject(contest);
        }

        [HttpGet]
        [Route("")]
        public string List()
        {
            var contests = contestManager.GetAll()
                                         .OrderByDescending(c => c.CreationDate)
                                         .Select(c => new { c.Title, c.Id })
                                         .ToArray();

            return JsonConvert.SerializeObject(contests);
        }

        [HttpGet]
        [Route("{id}/news")]
        public string GetNews(Guid id)
        {
            var news = contestManager.GetNews(id);

            return JsonConvert.SerializeObject(news);
        }

        [HttpGet]
        [Route("{id}/info")]
        public string Get(Guid id)
        {
            var contest = contestManager.Get(id);

            return JsonConvert.SerializeObject(contest);
        }

        [HttpPost]
        [Route("{id}/participate")]
        public void Participate(Guid id, FieldWithValue[] values)
        {
            var user = cookieManager.GetUser(Request);

            userManager.FillFields(user.Id, values);
            contestManager.AddParticipant(id, user.Id);
        }

        [HttpGet]
        [Route("{id}/results")]
        public string Results(Guid id)
        {
            var participants = contestManager.GetParticipants(id);
            
            return JsonConvert.SerializeObject(participants);
        }
    }
}