namespace Core
{
    public static class Constants
    {
#if DEBUG
        public const bool IsSecureCookie = false;
#else
        public const bool IsSecureCookie = true;
#endif
    }
}
