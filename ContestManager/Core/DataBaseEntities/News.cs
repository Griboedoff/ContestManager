using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DataBaseEntities
{
    public class News : DataBaseEntity
    {
        [ForeignKey("Contest")]
        public Guid ContestId { get; set; }
        
        public Contest Contest { get; set; }
        [Column]
        public DateTime CreationDate{ get; set; }

        [Column]
        public string MdContent{ get; set; }
    }
}