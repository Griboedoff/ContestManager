using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Core.Badges;
using Core.DataBase;
using Core.DataBaseEntities;
using Front.React.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Front.React.Controllers
{
    [Authorized]
    public class BadgesController : ControllerBase
    {
        private readonly IBadgesDrawer badgesDrawer;
        private readonly IAsyncRepository<Participant> participantsRepo;

        public BadgesController(IBadgesDrawer badgesDrawer, IAsyncRepository<Participant> participantsRepo)
        {
            this.badgesDrawer = badgesDrawer;
            this.participantsRepo = participantsRepo;
        }

        [HttpGet("print")]
        public async Task<ActionResult> PrintForParticipant(User user, Guid contestId)
        {
            var participant = await participantsRepo.FirstOrDefaultAsync(p => p.ContestId == contestId && p.UserId == user.Id);
            if (participant == null || !participant.Verified)
                return BadRequest();

            var pdf = badgesDrawer.DrawBadges(new List<BadgeData> { new BadgeData(participant) });
            using (var ms = new MemoryStream())
            {
                pdf.Save(ms, false);

                return File(ms.ToArray(), MediaTypeNames.Application.Pdf, $"{user.Name}.pdf");
            }
        }

        public ActionResult PrintForUser(User user)
        {
            var pdf = badgesDrawer.DrawBadges(
                new List<BadgeData>
                {
                    new BadgeData(user)
                });
            using (var ms = new MemoryStream())
            {
                pdf.Save(ms, false);

                return File(ms.ToArray(), MediaTypeNames.Application.Pdf, $"{user.Name}.pdf");
            }
        }
    }
}
