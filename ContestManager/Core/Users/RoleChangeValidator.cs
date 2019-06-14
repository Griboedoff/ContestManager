using Core.Enums.DataBaseEnums;
using Core.Extensions;

namespace Core.Users
{
    public static class RoleChangeValidator
    {
        public static bool Validate(UserRole from, UserRole to)
            => from.GetAttribute<ImportanceAttribute>().Value <= to.GetAttribute<ImportanceAttribute>().Value;
    }
}