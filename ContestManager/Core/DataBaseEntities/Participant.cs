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
        public ResultDescription[] Results
        {
            get => SerializedResults == null
                ? new ResultDescription[0]
                : JsonConvert.DeserializeObject<ResultDescription[]>(SerializedResults);
            set => SerializedResults = value == null
                ? null
                : JsonConvert.SerializeObject(value);
        }

        [JsonIgnore]
        public string SerializedUserSnapshot { get; set; }

        [NotMapped]
        public User UserSnapshot
        {
            get => JsonConvert.DeserializeObject<User>(SerializedUserSnapshot);
            set => SerializedUserSnapshot = JsonConvert.SerializeObject(value);
        }

        public Participant WithUser(User user) => new Participant
        {
            ContestId = ContestId,
            UserId = UserId,
            SerializedResults = SerializedResults,
            SerializedUserSnapshot = JsonConvert.SerializeObject(user),
        };
    }

    public class ResultDescription
    {
        public int TaskNumber { get; set; }

        public int Value { get; set; }
    }
}