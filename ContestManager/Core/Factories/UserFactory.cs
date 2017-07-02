using System;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;

namespace Core.Factories
{
    public interface IUserFactory
    {
        User Create(string userName, UserRole role);
    }

    public class UserFactory : IUserFactory
    {
        public User Create(string userName, UserRole role)
            => new User {Id = Guid.NewGuid(), Name = userName, Role = role};
    }
}