using System.Web.Mvc;
using Core.Managers;
using Core.Models;
using Newtonsoft.Json;

namespace Front.Controllers
{
    [RoutePrefix("contests")]
    public class ContestsController : Controller
    {
        private readonly ICookieManager cookieManager;
        private readonly IContestManager contestManager;

        public ContestsController(ICookieManager cookieManager, IContestManager contestManager)
        {
            this.cookieManager = cookieManager;
            this.contestManager = contestManager;
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
            var contests = contestManager.GetAll();

            return JsonConvert.SerializeObject(contests);
        }
    }
}