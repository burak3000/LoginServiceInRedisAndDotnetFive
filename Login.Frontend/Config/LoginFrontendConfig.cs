using Newtonsoft.Json;
using System;
using System.IO;

namespace Login.FrontendService.Config
{
    internal class LoginFrontendConfig
    {



        private static Lazy<LoginFrontendConfig> _instance = new Lazy<LoginFrontendConfig>(ReadLoginFrontendServiceConfig);
        public static LoginFrontendConfig Values => _instance.Value;

        private static string ConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "Login.FrontendService.Config.json");

        private LoginFrontendConfig()
        {
        }

        private static LoginFrontendConfig ReadLoginFrontendServiceConfig()
        {
            return JsonConvert.DeserializeObject<LoginFrontendConfig>(File.ReadAllText(ConfigFilePath));
        }

        public RedisSettings RedisSettings { get; set; }



    }
    internal class RedisSettings
    {
        public string RedisConnectionString { get; set; }
        public string AuthenticateFrontEndRequestsChannel { get; set; }
        public string AuthenticateFrontEndResponsesChannel { get; set; }
        public string AuthenticateBackEndRequestsChannel { get; set; }
        public string AuthenticateBackEndResponsesChannel { get; set; }
        public string CreateUserRequestsChannel { get; set; }
    }
}
