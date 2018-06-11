using Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Models
{
    public class FieldDescription
    {
        public string Title { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }
    }
}