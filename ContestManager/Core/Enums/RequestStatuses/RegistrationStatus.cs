namespace Core.Enums.RequestStatuses
{
    public enum RegistrationStatus
    {
        Success,
        EmailAlreadyUsed,
        RequestCreated,
        RequestAlreadyUsed,
        WrongConfirmationCode,
        VkIdAlreadyUsed,
    }
}