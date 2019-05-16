using System;
using System.ComponentModel.DataAnnotations;
using Core.Enums.DataBaseEnums;
using Core.Helpers;

namespace Core.DataBaseEntities
{
    public class EmailConfirmationRequest : DataBaseEntity
    {
        public Guid AccountId { get; set; }
        public ConfirmationType Type { get; set; }

        [MaxLength(FieldsLength.Email)]
        public string Email { get; set; }

        [MaxLength(FieldsLength.ConfirmationCode)]
        public string ConfirmationCode { get; set; }

        public bool IsUsed { get; set; }
    }
}