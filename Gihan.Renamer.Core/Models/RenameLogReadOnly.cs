using System;
using System.Globalization;

namespace Gihan.Renamer.Models
{
    public class RenameLogReadOnly
    {
        public string Before { get; }
        public string After { get; }
        public DateTime? DateTime { get; }

        public RenameLogReadOnly(RenameLog renameLog)
            :this(renameLog.Before, renameLog.After, renameLog.DateTime)
        {}

        public RenameLogReadOnly(string before, string after, DateTime? dateTime)
        {
            Before = before;
            After = after;
            DateTime = dateTime;
        }

        public static explicit operator RenameLogReadOnly(RenameLog log)
        {
            return new RenameLogReadOnly(log);
        }

        public override string ToString()
        {
            var result = $"\"{Before}\" => \"{After}\"";
            if(DateTime.HasValue)
                result += $" at {DateTime.Value.ToString(CultureInfo.GetCultureInfo("fa-IR"))}";
            return result;
        }
    }
}
