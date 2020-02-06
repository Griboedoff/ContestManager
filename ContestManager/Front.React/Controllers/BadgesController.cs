using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using Core.Badges;
using Core.DataBaseEntities;
using Front.React.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Front.React.Controllers
{
    [Authorized]
    public class BadgesController : ControllerBase
    {
        private readonly IBadgesDrawer badgesDrawer;

        public BadgesController(IBadgesDrawer badgesDrawer)
        {
            this.badgesDrawer = badgesDrawer;
        }

        public ActionResult PrintForParticipant(User user)
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
