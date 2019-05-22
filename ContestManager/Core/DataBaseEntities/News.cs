using System;

namespace Core.DataBaseEntities
{
    public class News : DataBaseEntity
    {
        public Guid ContestId { get; set; }

        public Contest Contest { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}