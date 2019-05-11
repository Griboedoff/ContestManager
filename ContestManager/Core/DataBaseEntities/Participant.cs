using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Core.DataBaseEntities
{
    public class Participant : DataBaseEntity
    {
        [Column]
        public Guid ContestId { get; set; }

        [Column]
        public Guid UserId { get; set; }

        [JsonIgnore]
        [Column]
        public string SerializedResults { get; set; }

        [NotMapped]
        public ResultDescription[] Results
        {
            get => SerializedResults == null
                ? new ResultDescription[0]
                : JsonConvert.DeserializeObject<ResultDescription[]>(SerializedResults);
            set => SerializedResults = value == null
                ? null
                : JsonConvert.SerializeObject(value);
        }
    }

    public class ResultDescription
    {
        public int TaskNumber { get; set; }

        public int Value { get; set; }
    }
}