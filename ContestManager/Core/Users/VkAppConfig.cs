using Core.Configs;

namespace Core.Users
{
    public class VkAppConfig : IConfig
    {
        public string AppId { get; set; }

        public string SecretKey { get; set; }

        public string ServiceAccessKey { get; set; }
    }
}