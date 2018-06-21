using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DataBaseEntities
{
    public class Participant : DataBaseEntity
    {
        [Column]
        public Guid ContestId { get; set; }

        [Column]
        public Guid UserId { get; set; }
    }
}