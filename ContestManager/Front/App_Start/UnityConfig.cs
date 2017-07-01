using System.Linq;
using System.Web.Mvc;
using Core.Factories;
using Core.Helpers;
using Core.Managers;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Core.DataBaseEntities.Configs;
using Core.Exceptions;

namespace Front
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<IUserFactory, UserFactory>();
            container.RegisterType<IServiceTokenFactory, ServiceTokenFactory>();
            container.RegisterType<IContextAdapterFactory, ContextAdapterFactory>();
            container.RegisterType<IAuthenticationAccountFactory, AuthenticationAccountFactory>();
            container.RegisterType<IEmailConfirmationRequestFactory, EmailConfirmationRequestFactory>();

            var contextFactory = new ContextAdapterFactory();
            using (var db = contextFactory.Create())
            {
                var emailConfigs = db.Set<EmailConfig>().ToArray();
                if (emailConfigs.Length != 1)
                    throw new ConfigNotConsistentException<EmailConfig>();

                container.RegisterInstance(typeof(EmailConfig), emailConfigs[0]);
            }

            container.RegisterType<ICryptoHelper, CryptoHelper>();
            container.RegisterType<IDataGenerator, DataGenerator>();

            container.RegisterType<IEmailManager, EmailManager>();
            container.RegisterType<IRegistrationManager, RegistrationManager>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}