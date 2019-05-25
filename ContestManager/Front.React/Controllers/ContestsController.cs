using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Contests;
using Core.Enums.DataBaseEnums;
using Core.Sessions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Front.React.Controllers
{
    public class ContestsController : BaseController
    {
        private readonly IUserCookieManager cookieManager;
        private readonly IContestManager contestManager;

        public ContestsController(IUserCookieManager cookieManager, IContestManager contestManager)
        {
            this.cookieManager = cookieManager;
            this.contestManager = contestManager;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateContestModel contestModel)
        {
            var user = await cookieManager.GetUser(Request);
            if (user.Role != UserRole.Admin)
                return StatusCode(403);

            var contest = await contestManager.Create(contestModel, user.Id);

            return Json(contest);
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
        public async Task<JsonResult> GetNews(Guid id)
        {
            var news = await contestManager.GetNews(id);

            return Json(news.OrderByDescending(n => n.CreationDate));
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(Guid id)
        {
            var contest = await contestManager.Get(id);

            return Json(contest);
        }

        [HttpPost("{id}/participate")]
        public async Task<ObjectResult> Participate(Guid id)
        {
            var user = await cookieManager.GetUser(Request);
            if (user.Role != UserRole.Participant)
                return StatusCode(400, "Неверная роль");
            if (string.IsNullOrEmpty(user.School))
                return StatusCode(400, "Не заполнена школа");
            if (!user.Class.HasValue)
                return StatusCode(400, "Не указан класс");

            await contestManager.AddParticipant(id, user.Id);
            return StatusCode(200, "Успешно");
        }

        [HttpGet("{id}/results")]
        public string Results(Guid id)
        {
            var participants = contestManager.GetParticipants(id);

            return JsonConvert.SerializeObject(participants);
        }
    }
}