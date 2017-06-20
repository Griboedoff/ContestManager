using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;

namespace Core.DataBaseEntities
{
    public class User : DataBaseEntity
    {
        [Column]
        public string Name { get; set; }

        [Column]
        public UserRole Role { get; set; }
    }
}