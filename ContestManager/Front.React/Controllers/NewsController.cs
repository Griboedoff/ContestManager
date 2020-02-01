using System;
using System.Threading.Tasks;
using Core.Contests;
using Core.Contests.News;
using Core.Enums.DataBaseEnums;
using Front.React.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Front.React.Controllers
{
    public class NewsController : ControllerBase
    {
        private readonly IContestManager contestManager;
        private readonly INewsManager newsManager;

        public NewsController(
            IContestManager contestManager,
            INewsManager newsManager)
        {
            this.contestManager = contestManager;
            this.newsManager = newsManager;
        }

        [HttpPost]
        [Authorized(UserRole.Admin)]
        public async Task<ActionResult> Add([FromBody] CreateNewsModel createNewsModel)
        {
            if (!await contestManager.Exists(createNewsModel.ContestId))
                return NotFound();

            return Json(await newsManager.Create(createNewsModel));
        }

        [HttpPatch("{id}")]
        [Authorized(UserRole.Admin)]
        public async Task<ActionResult> Add(Guid id, [FromBody] CreateNewsModel createNewsModel)
        {
            if (!await newsManager.Exists(id))
                return NotFound();

            return Json(await newsManager.Update(id, createNewsModel));
        }
    }
}
