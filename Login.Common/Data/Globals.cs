using System;

namespace Login.Common
{
    public class Globals
    {
        #region Global constants
        public const string RedisConnectionString = "localhost:6379";
        public const string AuthenticateFrontEndRequestsChannel = "authenticate_frontend_requests_channel";
        public const string AuthenticateFrontEndResponsesChannel = "authenticate_frontend_responses_channel";
        public const string AuthenticateBackEndRequestsChannel = "authenticate_backend_requests_channel";
        public const string AuthenticateBackEndResponsesChannel = "authenticate_backend_responses_channel";
        public const string CreateUserRequestsChannel = "create_user_requests_channel";
        public const string CreateUserResponseChannel = "create_user_responses_channel";
        #endregion
    }
}