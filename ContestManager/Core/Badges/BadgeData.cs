using System;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;

namespace Core.Badges
{
    public class BadgeData
    {
        public readonly User User;
        public readonly string Auditorium = "";
        public readonly string Login = "";

        public BadgeData(Participant participant)
        {
            User = participant.UserSnapshot;
            // Auditorium = participant.Auditorium;
            Login = participant.Login;
        }

        public BadgeData(User user)
        {
            User = user;
        }

        public bool HasTwoParts()
        {
            return User.Role == UserRole.Participant;
        }

        public string GetRoleName()
        {
            switch (User.Role)
            {
                case UserRole.Participant:
                    return "Участник";
                case UserRole.Coach:
                    return "Тренер";
                case UserRole.Volunteer:
                    return "Волонтер";
                case UserRole.Checker:
                    return "Проверяющий";
                case UserRole.Admin:
                    return "Организатор";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
