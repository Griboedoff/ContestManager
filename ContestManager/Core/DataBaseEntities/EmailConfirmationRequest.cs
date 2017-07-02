using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums.DataBaseEnums;
using Core.Helpers;

namespace Core.DataBaseEntities
{
    public class EmailConfirmationRequest : DataBaseEntity
    {
        [Column]
        [Index("EmailConfirmationRequest_Type_Email_ConfirmationCode_Index", Order = 1, IsClustered = false, IsUnique = true)]
        public ConfirmationType Type { get; set; }

        [Column]
        [MaxLength(FieldsLength.Email)]
        [Index("EmailConfirmationRequest_Type_Email_ConfirmationCode_Index", Order = 2, IsClustered = false, IsUnique = true)]
        public string Email { get; set; }

        [Column]
        [MaxLength(FieldsLength.ConfirmationCode)]
        [Index("EmailConfirmationRequest_Type_Email_ConfirmationCode_Index", Order = 3, IsClustered = false, IsUnique = true)]
        public string ConfirmationCode { get; set; }

        [Column]
        public bool IsUsed { get; set; }
    }
}