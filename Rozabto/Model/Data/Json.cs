using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model.Data
{
    public static class Json
    {
        public static T Read<T>(string file) =>
            File.Exists(file + ".json") ?
            JsonConvert.DeserializeObject<T>(File.ReadAllText(file + ".json")) : default(T);

        public static void Write<T>(T obj, string file)
        {
            File.WriteAllText(file + ".json",
                JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
