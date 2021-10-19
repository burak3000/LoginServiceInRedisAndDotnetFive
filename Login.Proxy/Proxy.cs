using System;
using System.Threading;
using Login.Common.Utilities;
using Login.Proxy.Config;
using StackExchange.Redis;


namespace Login.Proxy
{
    class Proxy
    {

        private static ConnectionMultiplexer _connection;
        private static string _authFeReqCh;
        private static string _authFeRespCh;
        private static string _authBeReqCh;
        private static string _authBeRespCh;
        private static ISubscriber _pubsub;


        public Proxy()
        {
        }
        static void Main(string[] args)
        {
            InitRedisSettings();
            Console.WriteLine("Proxy is up and running. Waiting login requests from frontend service...");

            //Proxy just sends the messages to the backend side to process request.
            _pubsub.Subscribe(_authFeReqCh, (channel, message) => HandleRequestMessage(message));

            //Proxy just sends the processed request message to the FE side.
            _pubsub.Subscribe(_authBeRespCh, (channel, message) => HandleResponseMessage(message));


            Thread.Sleep(Timeout.Infinite);
        }

        private static void InitRedisSettings()
        {
            _connection = ConnectionMultiplexer.Connect(LoginProxyConfig.Values.RedisSettings.RedisConnectionString);
            _authFeReqCh = LoginProxyConfig.Values.RedisSettings.AuthenticateFrontEndRequestsChannel;
            _authFeRespCh = LoginProxyConfig.Values.RedisSettings.AuthenticateFrontEndResponsesChannel;
            _authBeReqCh = LoginProxyConfig.Values.RedisSettings.AuthenticateBackEndRequestsChannel;
            _authBeRespCh = LoginProxyConfig.Values.RedisSettings.AuthenticateBackEndResponsesChannel;
            _pubsub = _connection.GetSubscriber();
        }


        static void HandleRequestMessage(string message)
        {
            try
            {
                Console.WriteLine("Proxy is handling the request.");
                RedisMessagingHelper.SendMessage(_pubsub, _authBeReqCh, message, CommandFlags.FireAndForget);
                System.Console.WriteLine("Waiting response from backend service.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        static void HandleResponseMessage(string message)
        {
            try
            {
                Console.WriteLine("Response is arrived. Proxy is sending result to front end service.");
                RedisMessagingHelper.SendMessage(_pubsub, _authFeRespCh, message, CommandFlags.FireAndForget);
                System.Console.WriteLine("Response is sent to front end.");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }


    }
}
