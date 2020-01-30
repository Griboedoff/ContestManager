using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Core.DataBaseEntities
{
    public class QualificationParticipation : DataBaseEntity
    {
        public Guid ContestId { get; set; }
        public Guid ParticipantId { get; set; }
        public DateTimeOffset EndTime { get; set; }

        [JsonIgnore]
        public string SerializedAnswers { get; set; }

        [NotMapped]
        public string[] Results
        {
            get => SerializedAnswers == null
                ? new string[0]
                : JsonConvert.DeserializeObject<string[]>(SerializedAnswers);
            set => SerializedAnswers = value == null
                ? null
                : JsonConvert.SerializeObject(value);
        }
    }
}
