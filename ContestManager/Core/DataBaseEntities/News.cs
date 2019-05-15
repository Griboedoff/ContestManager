using System;

namespace Core.DataBaseEntities
{
    public class News : DataBaseEntity
    {
        public Guid ContestId { get; set; }

        public Contest Contest { get; set; }

        public DateTime CreationDate{ get; set; }

        public string MdContent{ get; set; }
    }
}