using System.Linq;
using System.Web.Mvc;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Factories;
using Core.Helpers;
using Core.Managers;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Core.Exceptions;
using Core.Models.Configs;
using Newtonsoft.Json;

namespace Front
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            RegisterStoredConfigs(container);

            container.RegisterType<ICookieManager, CookieManager>();
            container.RegisterType<IContextAdapterFactory, ContextAdapterFactory>();
            container.RegisterType<IAuthenticationAccountFactory, AuthenticationAccountFactory>();
            container.RegisterType<IEmailConfirmationRequestFactory, EmailConfirmationRequestFactory>();

            container.RegisterType<ICryptoHelper, CryptoHelper>();
            container.RegisterType<IDataGenerator, DataGenerator>();


            container.RegisterType<IEmailManager, EmailManager>();
            container.RegisterType<ISecurityManager, SecurityManager>();
            container.RegisterType<IUserManager, UserManager>();
            container.RegisterType<IAuthenticationManager, AuthenticationManager>();
            container.RegisterType<IContestManager, ContestManager>();
            container.RegisterType<INewsManager, NewsManager>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static void RegisterStoredConfigs(IUnityContainer container)
        {
            var contextFactory = new ContextAdapterFactory();

            using (var db = contextFactory.Create())
            {
                var storedConfigs = db.Set<StoredConfig>().ToArray();

                RegisterStoredConfig<EmailConfig>(container, storedConfigs);
                RegisterStoredConfig<VkAppConfig>(container, storedConfigs);
                RegisterStoredConfig<RSACryptoConfig>(container, storedConfigs);
            }
        }

        private static void RegisterStoredConfig<T>(IUnityContainer container, StoredConfig[] storedConfigs)
            where T : class, IConfig
        {
            var typeName = typeof(T).Name;

            var wantedStoredConfigs = storedConfigs.Where(c => c.TypeName == typeName).ToArray();
            if (wantedStoredConfigs.Length != 1)
                throw new ConfigNotConsistentException<T>();

            var storedConfig = JsonConvert.DeserializeObject<T>(wantedStoredConfigs[0].JsonValue);
            container.RegisterInstance(storedConfig);
        }
    }
}