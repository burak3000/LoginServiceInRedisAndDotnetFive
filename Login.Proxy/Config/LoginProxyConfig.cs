using System;
using System.IO;
using Newtonsoft.Json;

namespace Login.Proxy.Config
{
    internal class LoginProxyConfig
    {



        private static Lazy<LoginProxyConfig> _instance = new Lazy<LoginProxyConfig>(ReadProxyConfig);
        public static LoginProxyConfig Values => _instance.Value;

        private static string ConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "Login.Proxy.Config.json");

        private LoginProxyConfig()
        {
        }

        private static LoginProxyConfig ReadProxyConfig()
        {
            return JsonConvert.DeserializeObject<LoginProxyConfig>(File.ReadAllText(ConfigFilePath));
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
