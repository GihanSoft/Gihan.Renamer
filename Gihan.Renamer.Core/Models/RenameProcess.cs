using Gihan.Helpers.String;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gihan.Renamer.Models
{
    public class RenameProcess
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public IEnumerable<string> Items { get; set; }
        public IEnumerable<ReplacePattern> Patterns { get; set; }
    }
}
