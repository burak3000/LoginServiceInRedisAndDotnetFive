using CommandLine;

namespace Login.FrontendService
{
    public class CmdLineArgs
    {
        [Option('a', "authenticate-user", Required = false, HelpText = "Authentication operation. For users who already created an account.")]
        public bool AuthenticateUser { get; set; }

        [Option('c', "create-new-user", Required = false, HelpText = "Create a new user.")]
        public bool CreateUser { get; set; }
    }

}