using StackExchange.Redis;

namespace Login.Common.Utilities
{
    public static class RedisMessagingHelper
    {
        public static void SendMessage(ISubscriber subscriber, string channel, string message, CommandFlags commandFlags)
        {
            System.Console.WriteLine("Sending message to channel: " + channel);
            System.Console.WriteLine("Message is: " + message);
            try
            {
                subscriber.PublishAsync(channel, message, commandFlags);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error when sending message.");
                System.Console.WriteLine(e.StackTrace);
            }
        }
    }
}