using System.ComponentModel.DataAnnotations;
using Core.Enums.DataBaseEnums;
using Core.Helpers;

namespace Core.DataBaseEntities
{
    public class User : DataBaseEntity
    {
        [MaxLength(FieldsLength.Name)]
        public string Name { get; set; }

        public UserRole Role { get; set; }
        public Sex Sex { get; set; }
        public Class? Class { get; set; }
        public string School { get; set; }
    }

    public enum Class
    {
        Fifth = 5,
        Sixth = 6,
        Seventh = 7,
        Eighth = 8,
        Ninth = 9,
        Tenth = 10,
        Eleventh = 11,
    }

    public enum Sex
    {
        Male,
        Female,
    }
}