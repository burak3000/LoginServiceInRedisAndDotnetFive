using Login.Common;
using Login.Common.Utilities;
using Login.FrontendService.Config;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Login.FrontendService
{
    public class ResponseHandler
    {
        private static ConnectionMultiplexer _authReqCh = ConnectionMultiplexer.Connect(LoginFrontendConfig.Values.RedisSettings.RedisConnectionString);

        private ISubscriber _authenticationPubsub;
        private TaskCompletionSource<string> _loginCompletionSource = new();



        public ResponseHandler()
        {
            _authenticationPubsub = _authReqCh.GetSubscriber();
            _authenticationPubsub.Subscribe(LoginFrontendConfig.Values.RedisSettings.AuthenticateFrontEndResponsesChannel,
     (channel, message) => _loginCompletionSource.SetResult(message));
        }
        public ResponseDto WaitAuthenticationResponseAndHandleResult(string userJson)
        {
            System.Console.WriteLine("Waiting response from proxy...");

            Task<string> startTask = _loginCompletionSource.Task;
            Task.WaitAll(new Task[] { startTask }, 15000);

            if (startTask.Result.Contains("Failed"))
            {
                System.Console.WriteLine("The timeout interval elapsed while waiting response from proxy.");
                return new ResponseDto() { RequestState = RequestState.Failed, AdditionalMessage = "5 seconds timeout is exceeded before response" };
            }
            else
            {
                var result = startTask.Result;
                System.Console.WriteLine("Response is arrived.");
                return JsonHelper.DeserializeFromString<ResponseDto>(result);
            }


        }
        //public async Task<string> ListenLoginResultAsync()
        //{
        //    string messageContent = await GetMessageAsync();

        //    return messageContent;
        //}

        //private Task<string> GetMessageAsync()
        //{
        //    var tcs = new TaskCompletionSource<string>();
        //    _authenticationPubsub.Subscribe(LoginFrontendConfig.Values.RedisSettings.AuthenticateFrontEndResponsesChannel,
        //        (channel, message) => tcs.TrySetResult(message));
        //    return tcs.Task;
        //}
    }
}