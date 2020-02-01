using System;

namespace Core.DataBaseEntities
{
    public class Session : DataBaseEntity
    {
        public Guid UserId { get; set; }
        public DateTimeOffset LastUse { get; set; }
    }
}
