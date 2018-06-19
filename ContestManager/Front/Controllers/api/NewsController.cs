using System;
using System.Web.Mvc;
using Core.Managers;
using Newtonsoft.Json;

namespace Front.Controllers.api
{
    [RoutePrefix("news")]
    public class NewsController : Controller
    {
        private readonly IContestManager contestManager;
        private readonly INewsManager newsManager;

        public NewsController(IContestManager contestManager, INewsManager newsManager)
        {
            this.contestManager = contestManager;
            this.newsManager = newsManager;
        }

        [HttpPost]
        [Route("")]
        public string Add(string mdContent, Guid contestId)
        {
            if (!contestManager.Exists(contestId))
                Response.StatusCode = 404;

            var news = newsManager.Create(mdContent, contestId);

            return JsonConvert.SerializeObject(news);
        }
    }
}