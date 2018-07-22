using System.Collections.Generic;
using Gihan.Renamer.Models;
using Gihan.Storage.SystemIO;

namespace Gihan.Renamer.SystemIO
{
    public class Renamer : Core.Renamer
    {
        public Renamer(Folder folder, IEnumerable<RenameRule> renameRules, bool includeExtension = false)
            : base(folder, renameRules, includeExtension)
        {
        }

        public Renamer(string folderPath, IEnumerable<RenameRule> renameRules, bool includeExtension = false)
            : base(new Folder(folderPath), renameRules, includeExtension)
        {
        }

        public static void Rename(File file, IEnumerable<RenameRule> rules, bool includeExtension = false)
        {
            Gihan.Renamer.Core.Renamer.Rename(file, rules, includeExtension);
        }

        public static void Rename(string filePath, IEnumerable<RenameRule> rules, bool includeExtension = false)
        {
            var file = new File(filePath);
            Rename(file, rules, includeExtension);
        }
    }
}
