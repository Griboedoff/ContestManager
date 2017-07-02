using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums.DataBaseEnums;
using Core.Helpers;

namespace Core.DataBaseEntities
{
    public class AuthenticationAccount : DataBaseEntity
    {
        [Column]
        public Guid UserId { get; set; }

        [Column]
        [Index("AuthenticationAccount_Type_ServiceId_Index", Order = 1, IsClustered = false, IsUnique = true)]
        public AuthenticationType Type { get; set; }

        [Column]
        [MaxLength(FieldsLength.ServiceId)]
        [Index("AuthenticationAccount_Type_ServiceId_Index", Order = 2, IsClustered = false, IsUnique = true)]
        public string ServiceId { get; set; }

        [Column]
        public string ServiceToken { get; set; }
    }
}