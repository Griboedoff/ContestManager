using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;
using Newtonsoft.Json;

namespace Core.DataBaseEntities
{
    public class Contest : DataBaseEntity
    {
        public string Title { get; set; }

        public Guid OwnerId { get; set; }

        public ContestType Type { get; set; }

        public ContestOptions Options { get; set; }

        public DateTime CreationDate { get; set; }

        [NotMapped]
        public Auditorium[] Auditoriums
        {
            get => AuditoriumsJson == null ? null : JsonConvert.DeserializeObject<Auditorium[]>(AuditoriumsJson);
            set => AuditoriumsJson = JsonConvert.SerializeObject(value);
        }

        [NotMapped]
        public Dictionary<Class, List<string>> TasksDescription
        {
            get => TasksDescriptionJson == null
                ? null
                : JsonConvert.DeserializeObject<Dictionary<Class, List<string>>>(TasksDescriptionJson);
            set => TasksDescriptionJson = JsonConvert.SerializeObject(value);
        }

        public string ResultsTableLink { get; set; }

        [JsonIgnore]
        public string AuditoriumsJson { get; set; }

        [JsonIgnore]
        public string TasksDescriptionJson { get; set; }
    }

    public class Auditorium
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Capacity { get; set; }
    }
}
