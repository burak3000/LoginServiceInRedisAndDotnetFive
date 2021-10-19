using Newtonsoft.Json;
using System;
using System.IO;

namespace Login.BackendService.Config
{
    internal class LoginBackendServiceConfig
    {
        private static Lazy<LoginBackendServiceConfig> _instance = new Lazy<LoginBackendServiceConfig>(ReadLoginBackendServiceConfig);
        public static LoginBackendServiceConfig Values => _instance.Value;

        private static string ConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "Login.BackendService.Config.json");

        private LoginBackendServiceConfig()
        {
        }

        private static LoginBackendServiceConfig ReadLoginBackendServiceConfig()
        {
            return JsonConvert.DeserializeObject<LoginBackendServiceConfig>(File.ReadAllText(ConfigFilePath));
        }

        public RedisSettings RedisSettings { get; set; }



    }
    internal class RedisSettings
    {
        public string RedisConnectionString { get; set; }
        public string AuthenticateBackEndRequestsChannel { get; set; }
        public string AuthenticateBackEndResponsesChannel { get; set; }
    }
}
