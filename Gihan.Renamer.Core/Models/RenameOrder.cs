using Newtonsoft.Json;

namespace Gihan.Renamer.Models
{
    public class RenameOrder
    {
        public string Path { get; set; }
        public string DestPath { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public static RenameOrder JsonDeserialize(string jsonObj)
        {
            return JsonConvert.DeserializeObject<RenameOrder>(jsonObj);
        }
        public static RenameOrder JsonArrayDeserialize(string jsonArr)
        {
            var arr = JsonConvert.DeserializeObject<string[]>(jsonArr);
            return new RenameOrder { Path = arr[0], DestPath = arr[1] };
        }
    }

    public static class RenameOrderEx
    {
        public static string JsonSerialize(this RenameOrder order)
        {
            return JsonConvert.SerializeObject(order);
        }
        public static string JsonArraySerialize(this RenameOrder order)
        {
            var arr = new[] { order.Path, order.DestPath };
            return JsonConvert.SerializeObject(arr);
        }
    }
}