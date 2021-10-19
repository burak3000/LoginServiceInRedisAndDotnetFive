using Login.BackendService.Config;
using Login.Common;
using Login.Common.Utilities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Login.BackendService
{

    class Backend
    {

        private static ConnectionMultiplexer _connection;
        private static string _authBeReqCh;
        private static string _authBeRespCh;
        private static ISubscriber _authenticationPubSub;

        private static Dictionary<string, IUser> _users;


        private static void InitRedisSettings()
        {
            string redisString = LoginBackendServiceConfig.Values.RedisSettings.RedisConnectionString;
            Console.WriteLine("Connecting to redis server at: " + redisString);
            _connection = ConnectionMultiplexer.Connect(redisString);
            _authBeReqCh = LoginBackendServiceConfig.Values.RedisSettings.AuthenticateBackEndRequestsChannel;
            _authBeRespCh = LoginBackendServiceConfig.Values.RedisSettings.AuthenticateBackEndResponsesChannel;
            _authenticationPubSub = _connection.GetSubscriber();
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("Backend service is starting...");
            InitRedisSettings();
            _users = ReadUsersFromPersistence();
            System.Console.WriteLine("Backend service is up and running.");

            System.Console.WriteLine("Waiting requests from proxy side.");
            //Proxy just sends the messages to the backend side to process request.
            _authenticationPubSub.Subscribe(_authBeReqCh, (channel, message) => HandleRequestMessage(message));

            Thread.Sleep(Timeout.Infinite);
        }
        static void HandleRequestMessage(string message)
        {
            RequestDto request = GetRequestDtoFromMessage(message);
            ResponseDto response = ValidateLogin(request);
            RedisMessagingHelper.SendMessage(_authenticationPubSub, _authBeRespCh, CreateResponseMessage(response), CommandFlags.FireAndForget);
        }

        private static ResponseDto ValidateLogin(RequestDto request)
        {
            System.Console.WriteLine("Validating login");
            if (_users.ContainsKey(request.UserInfo.UserName))
            {
                var alreadyCreatedUser = _users[request.UserInfo.UserName];
                if (alreadyCreatedUser.Password == request.UserInfo.Password)
                {
                    return new ResponseDto()
                    {
                        RequestState = RequestState.Successful,
                        UserInfo = alreadyCreatedUser,
                        AdditionalMessage = "User name and password matches."
                    };
                }
                else
                {
                    return new ResponseDto()
                    {
                        RequestState = RequestState.Failed,
                        AdditionalMessage = "User exists but password does not match!"
                    };
                }
            }
            return new ResponseDto()
            {
                RequestState = RequestState.Failed,
                AdditionalMessage = "This user does not exist in the system. First create a new account for this user."
            };
        }

        static RequestDto GetRequestDtoFromMessage(string message)
        {
            return JsonHelper.DeserializeFromString<RequestDto>(message);
        }

        static string CreateResponseMessage(ResponseDto response)
        {
            return JsonHelper.SerializeObjectToJson<ResponseDto>(response);
        }

        static Dictionary<string, IUser> ReadUsersFromPersistence()
        {
            System.Console.WriteLine("Reading users from persistent storage.");
            //Fake reading :)
            Dictionary<string, IUser> users = new Dictionary<string, IUser>();
            users.Add("burak", new UserBase() { UserName = "burak", Password = "pswd" });
            System.Console.WriteLine("Reading users is finished.");
            return users;
        }
    }
}
