using System;

namespace Gihan.Renamer.Models
{
    public class RenameLogReadOnly
    {
        public string Before { get; }
        public string After { get; }
        public DateTime DateTime { get; }

        public RenameLogReadOnly(RenameLog renameLog)
        {
            Before = renameLog.Before;
            After = renameLog.After;
            DateTime = renameLog.DateTime;
        }

        public RenameLogReadOnly(string before, string after, DateTime dateTime)
        {
            Before = before;
            After = after;
            DateTime = dateTime;
        }

        public static explicit operator RenameLogReadOnly(RenameLog log)
        {
            return new RenameLogReadOnly(log);
        }
    }
}
