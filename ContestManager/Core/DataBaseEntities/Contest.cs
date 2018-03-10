using System.ComponentModel.DataAnnotations.Schema;
using Core.Models;
using Newtonsoft.Json;

namespace Core.DataBaseEntities
{
    public class Contest : DataBaseEntity
    {
        [Column]
        public string Title { get; set; }

        [Column(TypeName = "jsonb")]
        public string SerializedFields { get; set; }

        [NotMapped]
        public FieldDescription[] Fields
        {
            get => SerializedFields == null
                ? new FieldDescription[0]
                : JsonConvert.DeserializeObject<FieldDescription[]>(SerializedFields);
            set => SerializedFields = value == null
                ? null :
                JsonConvert.SerializeObject(value);
        }
    }
}