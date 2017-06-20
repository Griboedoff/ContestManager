using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;

namespace Core.DataBaseEntities
{
    public class AuthenticationAccount : DataBaseEntity
    {
        [Column]
        public Guid UserId { get; set; }

        [Column]
        public AuthenticationType Type { get; set; }

        [Column]
        public string ServiceId { get; set; }

        [Column]
        public string ServiceToken { get; set; }
    }
}