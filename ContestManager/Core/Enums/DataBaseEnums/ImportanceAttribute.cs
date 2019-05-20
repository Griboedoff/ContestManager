using System;

namespace Core.Enums.DataBaseEnums
{
    public class ImportanceAttribute : Attribute
    {
        public readonly int Value;

        public ImportanceAttribute(int value) => this.Value = value;
    }
}