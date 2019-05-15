using System;
using System.ComponentModel.DataAnnotations;
using Core.Enums.DataBaseEnums;
using Core.Helpers;

namespace Core.DataBaseEntities
{
    public class AuthenticationAccount : DataBaseEntity
    {
        public Guid UserId { get; set; }

        public AuthenticationType Type { get; set; }

        [MaxLength(FieldsLength.ServiceId)]
        public string ServiceId { get; set; }

        public string ServiceToken { get; set; }
    }
}