using System.Threading.Tasks;
using Core.Contests;
using Core.Enums.DataBaseEnums;
using Core.News_;
using Core.Users.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Front.React.Controllers
{
    public class NewsController : ControllerBase
    {
        private readonly IContestManager contestManager;
        private readonly INewsManager newsManager;
        private readonly IUserCookieManager cookieManager;

        public NewsController(
            IContestManager contestManager,
            INewsManager newsManager,
            IUserCookieManager cookieManager)
        {
            this.contestManager = contestManager;
            this.newsManager = newsManager;
            this.cookieManager = cookieManager;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreateNewsModel createNewsModel)
        {
            var user = await cookieManager.GetUser(Request);
            if (user.Role != UserRole.Admin)
                return StatusCode(403);

            if (!await contestManager.Exists(createNewsModel.ContestId))
                return NotFound();

            var news = await newsManager.Create(createNewsModel);

            return Json(news);
        }
    }
}