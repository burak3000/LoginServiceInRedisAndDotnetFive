using CommandLine;
using StackExchange.Redis;
using System.Threading;

namespace Login.FrontendService
{
    class Frontend
    {
        private const string RedisConnectionString = "localhost:6379";
        private static ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(RedisConnectionString);
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CmdLineArgs>(args)
               .WithParsed<CmdLineArgs>(cliArgs =>
               {
                   if (!ValidateCmdLineArgs(cliArgs))
                   {
                       return;
                   }
                   RequestManager requestManager = new RequestManager();
                   requestManager.HandleParameters(cliArgs);

                   Thread.Sleep(Timeout.Infinite);
               });
        }
        static bool ValidateCmdLineArgs(CmdLineArgs cmdLineArgs)
        {
            if (cmdLineArgs.AuthenticateUser == cmdLineArgs.CreateUser)
            {
                System.Console.WriteLine("Invalid parameter. Please specify either authenticate or create new user.");
                return false;
            }
            return true;
        }
    }
}
