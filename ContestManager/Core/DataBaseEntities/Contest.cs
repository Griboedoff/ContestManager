using System;
using Core.Enums;

namespace Core.DataBaseEntities
{
    public class Contest : DataBaseEntity
    {
        public string Title { get; set; }

        public Guid OwnerId { get; set; }

        public ContestType Type { get; set; }

        public ContestOptions Options { get; set; }

        public DateTime CreationDate { get; set; }
    }
}