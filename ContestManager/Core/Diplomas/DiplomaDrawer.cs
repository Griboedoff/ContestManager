using System.Collections.Generic;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Core.Diplomas
{
    public interface IDiplomaDrawer
    {
        PdfDocument DrawDiplomas(IEnumerable<Diploma> diplomas);
    }

    public class DiplomaDrawer : IDiplomaDrawer
    {
        private static readonly Dictionary<int, XFont> Fonts;

        static DiplomaDrawer()
        {
            var opt = new XPdfFontOptions(PdfFontEncoding.Unicode);
            Fonts = new Dictionary<int, XFont>();
            foreach (var fontSize in new[] { 13, 18, 20, 23, 30 })
                Fonts[fontSize] = new XFont("Times New Roman", 13, XFontStyle.Bold, opt);
        }

        public PdfDocument DrawDiplomas(IEnumerable<Diploma> diplomas)
        {
            var doc = new PdfDocument();
            foreach (var diploma in diplomas)
                DrawDiplomaToPage(doc.AddPage(), diploma);
            return doc;
        }

        private void DrawText(
            XGraphics xgr,
            string text,
            XFont font,
            XPoint start,
            XPoint size,
            XStringFormat format)
        {
            var drawPos = new XRect(start.X, start.Y, size.X, font.Height);
            xgr.DrawString(text, font, XBrushes.Black, drawPos, format);
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
            for (var i = 0; i < arrName.Length; ++i)
            {
                DrawText(xgr, arrName[i], font, nstart, size, format);
                nstart.Y += font.Height - 3;
            }
        }

        private void DrawDiplomaToPage(PdfPage page, Diploma diploma)
        {
            var start = new XPoint(0, 270);
            var size = new XPoint(page.Width, page.Height);

            using (var xgr = XGraphics.FromPdfPage(page))
            {
                // if (!string.IsNullOrEmpty(diploma.TemplatePath))
                // {
                    // var img = XImage.FromFile(path + diploma.TemplatePath);
                    // xgr.DrawImage(img, 0, 0, page.Width, page.Height);
                // }

                DrawName(xgr, diploma.Name, Fonts[30], start, size, XStringFormats.Center);
                start.Y += 105;

                if (!string.IsNullOrEmpty(diploma.Type))
                    DrawText(xgr, diploma.Type, Fonts[23], start, size, XStringFormats.Center);

                start.Y += 30;

                if (!string.IsNullOrEmpty(diploma.Institution))
                {
                    DrawText(xgr, diploma.Institution, Fonts[23], start, size, XStringFormats.Center);
                    start.Y += 30;
                }

                if (!string.IsNullOrEmpty(diploma.City))
                {
                    DrawText(xgr, diploma.City, Fonts[23], start, size, XStringFormats.Center);
                    start.Y += 30;
                }

                if (!string.IsNullOrEmpty(diploma.Text))
                    DrawText(xgr, diploma.Text, Fonts[30], start, size, XStringFormats.Center);
            }
        }
    }
}
