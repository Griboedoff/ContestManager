using System.Runtime.Serialization;
using Core.Enums;

namespace Core.Models
{
    [DataContract]
    public class FieldDescription
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public ValidatorType ValidatorTypes { get; set; }
    }
}