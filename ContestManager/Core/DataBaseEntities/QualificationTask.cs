using System;

namespace Core.DataBaseEntities
{
    public class QualificationTask : DataBaseEntity
    {
        public int Number { get; set; }
        public byte[] Image { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public Class[] ForClasses { get; set; }
        public Guid ContestId { get; set; }
    }
}