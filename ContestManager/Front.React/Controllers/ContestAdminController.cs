using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Contests;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.SheetsApi;
using Front.React.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Front.React.Controllers
{
    [Authorized(UserRole.Admin)]
    public class ContestAdminController : ControllerBase
    {
        private readonly IContestManager contestManager;
        private readonly IAsyncRepository<Contest> contestsRepo;
        private readonly ISheetsApiClient sheetsApiClient;

        public ContestAdminController(
            IContestManager contestManager,
            IAsyncRepository<Contest> contestsRepo,
            ISheetsApiClient sheetsApiClient)
        {
            this.contestManager = contestManager;
            this.contestsRepo = contestsRepo;
            this.sheetsApiClient = sheetsApiClient;
        }

        [HttpPost("{id}/calcQualificationResults")]
        public async Task<ActionResult> CalculateQualificationResults(Guid id)
        {
            await contestManager.CalculateQualificationResults(id);

            var participants = await contestManager.GetParticipants(id);
            var groupByClass = participants.GroupBy(p => p.UserSnapshot.Class.Value)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(p => p.Results.Select(int.Parse).Sum()).ThenBy(p => p.UserSnapshot.Name).ToList());

            return Json(participants);
        }
    }
}
