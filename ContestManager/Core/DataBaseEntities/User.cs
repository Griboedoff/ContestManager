using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums.DataBaseEnums;
using Core.Helpers;

namespace Core.DataBaseEntities
{
    public class User : DataBaseEntity
    {
        [Column]
        [MaxLength(FieldsLength.Name)]
        public string Name { get; set; }

        [Column]
        public UserRole Role { get; set; }
    }
}