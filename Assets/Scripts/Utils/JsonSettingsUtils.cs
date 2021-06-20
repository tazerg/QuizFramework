using Newtonsoft.Json;

namespace QuizFramework.Utils
{
    public static class JsonSettingsUtils
    {
        public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };
    }
}