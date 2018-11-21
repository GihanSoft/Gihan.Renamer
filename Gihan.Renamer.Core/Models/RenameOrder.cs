using Newtonsoft.Json;

namespace Gihan.Renamer.Models
{
    public class RenameOrder
    {
        public string FilePath { get; set; }
        public string NewName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public static RenameOrder JsonDeserialize(string jsonObj)
        {
            return JsonConvert.DeserializeObject<RenameOrder>(jsonObj);
        }
        public static RenameOrder JsonArrayDeserialize(string jsonArr)
        {
            var arr = JsonConvert.DeserializeObject<string[]>(jsonArr);
            return new RenameOrder { FilePath = arr[0], NewName = arr[1] };
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
            var arr = new[] { order.FilePath, order.NewName };
            return JsonConvert.SerializeObject(arr);
        }
    }
}