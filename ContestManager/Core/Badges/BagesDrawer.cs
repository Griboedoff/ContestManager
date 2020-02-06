using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Core.Badges
{
    public interface IBadgesDrawer
    {
        PdfDocument DrawBadges(List<BadgeData> badges);
    }

    public class BadgesDrawer : IBadgesDrawer
    {
        private static readonly Dictionary<int, XFont> Fonts;

        private readonly string badgeImagePath;
        private string OrgPath => $"{badgeImagePath}/BOrg.bmp";
        private string TemplatePath => $"{badgeImagePath}/BTemplate.bmp";

        static BadgesDrawer()
        {
            var opt = new XPdfFontOptions(PdfFontEncoding.Unicode);
            Fonts = new Dictionary<int, XFont>();
            foreach (var fontSize in new[] { 13, 18, 20, 23, 30 })
                Fonts[fontSize] = new XFont("Times New Roman", 13, XFontStyle.Bold, opt);
        }

        public BadgesDrawer(IOptions<BadgeDrawerConfig> config )
        {
            badgeImagePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{config.Value.ImagePath}";
        }

        public PdfDocument DrawBadges(List<BadgeData> badges)
        {
            var doc = new PdfDocument();
            var page = doc.AddPage();
            var top = new XPoint(45, 28);
            var size = new XPoint(page.Width, page.Height);
            var badgeSize = GetBadgeSize();

            foreach (var badge in badges)
            {
                var mult = badge.HasTwoParts() ? 2 : 1;
                if (top.X + badgeSize.X * mult >= size.X)
                    top = new XPoint(45, 28 + badgeSize.Y - 1.5);

                if (top.Y + badgeSize.Y >= size.Y)
                {
                    page = doc.AddPage();
                    top = new XPoint(45, 28);
                }

                using (var xgr = XGraphics.FromPdfPage(page))
                {
                    DrawBadge(xgr, top, badgeSize, badge);

                    if (badge.HasTwoParts() || top.X > 45)
                        top = new XPoint(45, top.Y + badgeSize.Y - 1.5);
                    else
                        top.X += badgeSize.X - 1.5;
                }
            }

            return doc;
        }

        private XPoint GetBadgeSize()
        {
            var img = XImage.FromFile(TemplatePath);
            return new XPoint(img.PointWidth, img.PointHeight);
        }

        private void DrawTemplate(XGraphics xgr, XPoint start)
        {
            var img = XImage.FromFile(TemplatePath);
            xgr.DrawImage(img, start);
        }

        private void DrawOrg(XGraphics xgr, XPoint start, XPoint size)
        {
            var img = XImage.FromFile(OrgPath);
            start.Y += size.Y - img.PointHeight - 10;
            start.X += (size.X - img.PointWidth) / 2;
            xgr.DrawImage(img, start);
        }

        private void DrawText(
            XGraphics xgr,
            string text,
            XFont font,
            XPoint pos,
            XPoint size,
            XStringFormat format)
        {
            var drawPos = new XRect(pos.X, pos.Y, size.X, font.Height);
            xgr.DrawString(text, font, XBrushes.Black, drawPos, format);
        }

        private void DrawRole(
            XGraphics xgr,
            string role,
            XFont font,
            XPoint pos,
            XPoint size,
            XStringFormat format)
        {
            DrawText(xgr, role, font, new XPoint(pos.X, pos.Y + size.Y / 2 + 8), size, format);
        }

        private void DrawName(
            XGraphics xgr,
            string name,
            XFont font,
            XPoint start,
            XPoint size,
            XStringFormat format)
        {
            var arrName = name.Split(new[] { ' ' }, 2);
            var nstart = new XPoint(start.X, start.Y + 37);
            foreach (var parts in arrName)
            {
                DrawText(xgr, parts, font, nstart, size, format);
                nstart.Y += font.Height - 3;
            }
        }

        private void DrawParticipantDefText(XGraphics xgr, XPoint start, XPoint size)
        {
            DrawText(
                xgr,
                "Шифр участника:",
                Fonts[18],
                new XPoint(start.X, start.Y + 39),
                size,
                XStringFormats.Center);

            DrawText(
                xgr,
                "Аудитория:",
                Fonts[18],
                new XPoint(start.X, start.Y + 93),
                size,
                XStringFormats.Center);
        }

        private void DrawParticipantData(XGraphics xgr, XPoint start, XPoint size, BadgeData p)
        {
            var nstart = new XPoint(start.X, start.Y + 39 + Fonts[20].Height - 2);
            DrawText(
                xgr,
                string.Join(" ", (p.Login ?? "").ToArray()),
                Fonts[20],
                nstart,
                size,
                XStringFormats.Center);

            nstart = new XPoint(start.X, start.Y + 93 + Fonts[20].Height - 2);
            DrawText(xgr, p.Auditorium ?? "", Fonts[20], nstart, size, XStringFormats.Center);
        }

        private void DrawBadge(XGraphics xgr, XPoint pos, XPoint size, BadgeData data)
        {
            DrawTemplate(xgr, pos);
            DrawOrg(xgr, pos, size);
            DrawName(xgr, data.User.Name, Fonts[18], pos, size, XStringFormats.Center);
            DrawRole(xgr, data.GetRoleName(), Fonts[13], pos, size, XStringFormats.Center);

            if (!data.HasTwoParts()) return;

            pos = new XPoint(pos.X + size.X - 1.5, pos.Y);
            DrawTemplate(xgr, pos);
            DrawParticipantDefText(xgr, pos, size);
            DrawParticipantData(xgr, pos, size, data);
        }
    }
}
