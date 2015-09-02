using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// JSON相关功能
    /// </summary>
    public class JsonUtil
    {
        public static T FromJson<T>(string jsonString)
        {
            if (jsonString == null || "".Equals(jsonString))
            {
                return default(T);
            }
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }

        public static T parse<T>(string jsonString)
        {
            return FromJson<T>(jsonString);
        }

        public static string ToJson(object jsonObject)
        {
            if (jsonObject == null)
            {
                return "";
            }
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(jsonObject.GetType()).WriteObject(ms, jsonObject);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        } 
    }
}
