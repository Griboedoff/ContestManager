using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace Core.DataBaseEntities
{
    public class QualificationTask : DataBaseEntity
    {
        public int Number { get; set; }
        public byte[] Image { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }

        [JsonIgnore]
        public int[] ForClasses { get; set; }

        [NotMapped]
        public Class[] Classes
        {
            get => ForClasses?.Select(c => (Class) c).ToArray();
            set => ForClasses = value?.Select(c => (int)c).ToArray();
        }

        public Guid ContestId { get; set; }
    }
}
