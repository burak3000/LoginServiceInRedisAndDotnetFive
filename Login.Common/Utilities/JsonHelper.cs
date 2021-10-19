using Newtonsoft.Json;

namespace Login.Common.Utilities
{
    public static class JsonHelper
    {
        public static T DeserializeFromString<T>(string message)
        {
            var simpleDto = JsonConvert.DeserializeObject<T>(message,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            return simpleDto;
        }

        public static string SerializeObjectToJson<T>(T obj)
        {
            string dtoJson = JsonConvert.SerializeObject(obj,
                            new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            return dtoJson;
        }
    }
}