using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DataBaseEntities
{
    public class StoredConfig : DataBaseEntity
    {
        [Column]
        public string TypeName { get; set; }

        [Column]
        public string JsonValue { get; set; }
    }
}