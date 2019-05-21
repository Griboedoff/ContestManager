namespace Core.Enums.DataBaseEnums
{
    public enum UserRole
    {
        [Importance(0)]
        Participant = 1,
        [Importance(0)]
        Coach = 2,
        [Importance(1)]
        Volunteer = 3,
        [Importance(1)]
        Checker = 4,

        [Importance(100)]
        Admin = int.MaxValue
    }
}