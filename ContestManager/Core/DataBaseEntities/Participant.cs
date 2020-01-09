using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Core.DataBaseEntities
{
    public class Participant : DataBaseEntity
    {
        public Guid ContestId { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore]
        public string SerializedResults { get; set; }

        [NotMapped]
        public string[] Results
        {
            get => SerializedResults == null
                ? new string[0]
                : JsonConvert.DeserializeObject<string[]>(SerializedResults);
            set => SerializedResults = value == null
                ? null
                : JsonConvert.SerializeObject(value);
        }

        [JsonIgnore]
        public string SerializedUserSnapshot { get; set; }

        [NotMapped]
        public User UserSnapshot
        {
            get => SerializedUserSnapshot == null ? null : JsonConvert.DeserializeObject<User>(SerializedUserSnapshot);
            set => SerializedUserSnapshot = JsonConvert.SerializeObject(value);
        }

        public string Login { get; set; }

        public string Pass { get; set; }

        public string Verification { get; set; }

        public bool Verified { get; set; }
        public Participant WithoutLogin() => new Participant
        {
            Id = Id,
            ContestId = ContestId,
            UserId = UserId,
            SerializedResults = SerializedResults,
            SerializedUserSnapshot = SerializedUserSnapshot,
        };
    }
}
