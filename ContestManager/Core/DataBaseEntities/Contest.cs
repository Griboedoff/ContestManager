using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;
using Core.Models;
using Newtonsoft.Json;

namespace Core.DataBaseEntities
{
    public class Contest : DataBaseEntity
    {
        private const string TitleUniqueIndexName = "Contest_Title_Unique";

        [Column]
        [Index(TitleUniqueIndexName, IsClustered = false, IsUnique = true)]
        public string Title { get; set; }

        [Column]
        public Guid OwnerId { get; set; }

        [Column]
        public ContestOptions Options { get; set; }

        [JsonIgnore]
        [Column]
        public string SerializedFields { get; set; }

        [NotMapped]
        public FieldDescription[] Fields
        {
            get => SerializedFields == null
                ? new FieldDescription[0]
                : JsonConvert.DeserializeObject<FieldDescription[]>(SerializedFields);
            set => SerializedFields = value == null
                ? null
                : JsonConvert.SerializeObject(value);
        }
    }
}