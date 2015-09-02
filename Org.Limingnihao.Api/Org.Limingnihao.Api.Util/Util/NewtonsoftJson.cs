using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    public class NewtonsoftJson
    {
        public static T FromJson<T>(string jsonString)
        {
            if (jsonString == null || "".Equals(jsonString))
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
        }

        public static string ToJson(object jsonObject)
        {
            if (jsonObject == null)
            {
                return "";
            }
            return JsonConvert.SerializeObject(jsonObject);
        } 

    }
}
