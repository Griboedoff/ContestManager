namespace Core.Users.Sessions
{
    public enum ValidateUserSessionStatus
    {
        Ok,
        NoSidCookie,
        BadUserCookie,
        InvalidSession
    }
}
