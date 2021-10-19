using Login.Common;
using System;

namespace Login.FrontendService
{
    public class RequestManager
    {
        private RequestCreator _rqCreator;
        private ResponseHandler _listener;

        public RequestManager()
        {
            _rqCreator = new RequestCreator();
            _listener = new ResponseHandler();
        }
        public void HandleParameters(CmdLineArgs args)
        {
            System.Console.WriteLine("Executable is called with following args:");
            System.Console.WriteLine("Autchenticate user: " + args.AuthenticateUser);
            System.Console.WriteLine("Create new user: " + args.CreateUser);

            try
            {
                var userJson = _rqCreator.CreateUserInfoAndSendRequest(args);

                ResponseDto result = _listener.WaitAuthenticationResponseAndHandleResult(userJson);

                System.Console.WriteLine("Request type: " + result.RequestType);
                System.Console.WriteLine("Request state: " + result.RequestState);
                System.Console.WriteLine("Message: " + result.AdditionalMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }



        }
    }
}