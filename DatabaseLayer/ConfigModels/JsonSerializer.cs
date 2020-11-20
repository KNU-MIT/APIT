using System.IO;
using Newtonsoft.Json;

namespace DatabaseLayer.ConfigModels
{
    public static class JsonSerializer
    {
        internal static void Serialize<T>(this T arg, string fileName)
        {
            string res = JsonConvert.SerializeObject(arg, Formatting.Indented);
            File.WriteAllText(fileName, res);
        }

        internal static T Deserialize<T>(string fileName)
        {
            string json = File.ReadAllText(fileName);
            var res = JsonConvert.DeserializeObject<T>(json);
            return res;
        }
    }
}