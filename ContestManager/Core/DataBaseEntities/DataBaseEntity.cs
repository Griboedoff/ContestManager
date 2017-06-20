using System;
using System.ComponentModel.DataAnnotations;

namespace Core.DataBaseEntities
{
    public class DataBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
