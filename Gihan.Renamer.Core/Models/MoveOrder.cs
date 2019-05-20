using Newtonsoft.Json;

namespace Gihan.Renamer.Models
{
    public class MoveOrder
    {
        public string Path { get; set; }
        public string DestPath { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public static MoveOrder JsonDeserialize(string jsonObj)
        {
            return JsonConvert.DeserializeObject<MoveOrder>(jsonObj);
        }
        public static MoveOrder JsonArrayDeserialize(string jsonArr)
        {
            var arr = JsonConvert.DeserializeObject<string[]>(jsonArr);
            return new MoveOrder { Path = arr[0], DestPath = arr[1] };
        }
    }

    public static class RenameOrderEx
    {
        public static string JsonSerialize(this MoveOrder order)
        {
            return JsonConvert.SerializeObject(order);
        }
        public static string JsonArraySerialize(this MoveOrder order)
        {
            var arr = new[] { order.Path, order.DestPath };
            return JsonConvert.SerializeObject(arr);
        }
    }
}