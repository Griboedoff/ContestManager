using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Core.Enums.DataBaseEnums;
using Core.Helpers;
using Core.Models;
using Newtonsoft.Json;

namespace Core.DataBaseEntities
{
    [DataContract]
    public class User : DataBaseEntity
    {
        [Column]
        [DataMember]
        [MaxLength(FieldsLength.Name)]
        public string Name { get; set; }

        [Column]
        [DataMember]
        public UserRole Role { get; set; }

        [JsonIgnore]
        [Column]
        public string SerializedFields { get; set; }

        [NotMapped]
        public FieldWithValue[] Fields
        {
            get => SerializedFields == null
                ? new FieldWithValue[0]
                : JsonConvert.DeserializeObject<FieldWithValue[]>(SerializedFields);
            set => SerializedFields = value == null
                ? null
                : JsonConvert.SerializeObject(value);
        }
    }
}