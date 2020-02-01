namespace Core.Users.Sessions
{
    public enum ValidateUserSessionStatus
    {
        Ok,
        BadSidCookie,
        BadUserCookie,
        InvalidSession
    }
}
