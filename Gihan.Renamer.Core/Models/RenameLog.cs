using System;
using System.Globalization;

namespace Gihan.Renamer.Models
{
    public class RenameLog
    {
        public string Before { get; set; }
        public string After { get; set; }
        public DateTime? DateTime { get; set; }

        public RenameLog()
        {
            Before = null;
            After = null;
            DateTime = null;
        }

        public override string ToString()
        {
            return ((RenameLogReadOnly) this).ToString();
        }
    }
}
