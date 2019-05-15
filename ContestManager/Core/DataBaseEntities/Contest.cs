using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;
using Core.Models;
using Newtonsoft.Json;

namespace Core.DataBaseEntities
{
    public class Contest : DataBaseEntity
    {
        public string Title { get; set; }

        public Guid OwnerId { get; set; }

        public ContestOptions Options { get; set; }

        public ContestState State { get; set; }

        public DateTime CreationDate { get; set; }

        [JsonIgnore]
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

    public enum ContestState
    {
        RegistrationOpen = 0,
        RegistrationClosed = 100,
        Running = 200,
        Checking = 300,
        Finished = 400,
    }
}