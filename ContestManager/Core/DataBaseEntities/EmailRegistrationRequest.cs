using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Helpers;

namespace Core.DataBaseEntities
{
    public class EmailRegistrationRequest : DataBaseEntity
    {
        [Column]
        [MaxLength(FieldsLength.Name)]
        public string Name { get; set; }

        [Column]
        [MaxLength(FieldsLength.Email)]
        [Index("EmailRegistrationRequest_EmailIndex", IsClustered = false, IsUnique = true)]
        public string Email { get; set; }

        [Column]
        [MaxLength(FieldsLength.PasswordHash)]
        public byte[] PasswordHash { get; set; }

        [Column]
        [MaxLength(FieldsLength.Sult)]
        public string Sult { get; set; }

        [Column]
        [MaxLength(FieldsLength.Secret)]
        public string Secret { get; set; }

        [Column]
        public bool IsUsed { get; set; }
    }
}