using System.Web.Mvc;
using Core.Managers;
using Newtonsoft.Json;

namespace Front.Controllers
{
    public class ContestController : Controller
    {
        private readonly IContestManager contestManager;
        private readonly ICookieManager cookieManager;

        public ContestController(IContestManager contestManager, ICookieManager cookieManager)
        {
            this.contestManager = contestManager;
            this.cookieManager = cookieManager;
        }

        [HttpGet]
        public ActionResult Index() => View();

        [HttpPost]
        public string Create(string title)
        {
            var user = cookieManager.GetUser(Request);
            var contest = contestManager.Create(title, user.Id);

            return JsonConvert.SerializeObject(contest);
        }
    }
}