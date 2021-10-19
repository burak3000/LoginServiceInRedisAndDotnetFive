using Login.Common;
using Login.Common.Utilities;
using Login.FrontendService.Config;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace Login.FrontendService
{
    public class RequestCreator
    {

        private static ConnectionMultiplexer _authReqCh = ConnectionMultiplexer.Connect(LoginFrontendConfig.Values.RedisSettings.RedisConnectionString);

        private ISubscriber _pubsub;

        public RequestCreator()
        {
            _pubsub = _authReqCh.GetSubscriber();
        }

        public string CreateUserInfoAndSendRequest(CmdLineArgs args)
        {
            if (args.AuthenticateUser)
            {
                return HandleAuthenticationRequest();
            }
            else
                return HandleCreateNewUserRequest();

        }

        private string HandleAuthenticationRequest()
        {
            System.Console.WriteLine("Authentication request is came. Creating request.");
            string dtoJson = CreateRequestDto(RequestType.Authenticate);
            System.Console.WriteLine("Request created with json: " + dtoJson);

            System.Console.WriteLine("Sending request to: " + LoginFrontendConfig.Values.RedisSettings.AuthenticateFrontEndRequestsChannel);
            RedisMessagingHelper.SendMessage(_pubsub, LoginFrontendConfig.Values.RedisSettings.AuthenticateFrontEndRequestsChannel, dtoJson, CommandFlags.FireAndForget);

            return dtoJson;
        }

        private string HandleCreateNewUserRequest()
        {
            //TODO implement here...
            return null;
        }

        string CreateRequestDto(RequestType type)
        {
            IUser user = CreateUserFromUserInput();
            RequestDto request = new RequestDto()
            {
                RequestGuid = new Guid(),
                RequestState = RequestState.NotStarted,
                UserInfo = user,
                RequestType = type
            };

            string dtoJson = JsonConvert.SerializeObject(request,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            return dtoJson;
        }

        IUser CreateUserFromUserInput()
        {
            System.Console.WriteLine("Enter user name");
            string userName = System.Console.ReadLine();

            System.Console.WriteLine("Enter password");
            string password = System.Console.ReadLine();

            return new UserBase() { UserName = userName, Password = password, UserGuid = new Guid() };
        }
    }
}