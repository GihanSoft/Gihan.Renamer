using Gihan.Helpers.String;
using Gihan.Renamer.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gihan.Renamer.Models
{
    public class RenameProcess
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public RenameFlags RenameFlags { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RootFolder { get; set; }

        public IEnumerable<string> Items { get; set; }
        public IEnumerable<ReplacePattern> Patterns { get; set; }
    }
}
