using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Front.Helpers
{
    public class JsonNetResult : JsonResult
    {
        private readonly JsonSerializerSettings settings;

        public JsonNetResult()
        {
            settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Error };
        }

        public JsonNetResult(object data)
        {
            settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error
            };
            Data = data;
        }

        public bool Format
        {
            get { return settings.Formatting == Formatting.Indented; }
            set { settings.Formatting = value ? Formatting.Indented : Formatting.None; }
        }


        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data == null)
                return;

            var serializer = JsonSerializer.Create(settings);
            using (var sw = new StringWriter())
            {
                serializer.Serialize(sw, Data);
                response.Write(sw.ToString());
            }
        }
    }
}