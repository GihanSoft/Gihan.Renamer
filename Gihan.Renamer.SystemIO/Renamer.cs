using System.Collections.Generic;
using Gihan.Renamer.Models;
using Gihan.Storage.SystemIO;
using Gihan.Renamer.Ex;

namespace Gihan.Renamer.SystemIO
{
    public class Renamer : Gihan.Renamer.Renamer
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
            if (includeExtension)
            {
                file.Rename(file.Name.ReplaceRules(rules));
            }
            else
            {
                file.RenameIgnoreExtension(file.PureName.ReplaceRules(rules));
            }
        }

        public static void Rename(string filePath, IEnumerable<RenameRule> rules, bool includeExtension = false)
        {
            var file = new File(filePath);
            if (includeExtension)
            {
                file.Rename(file.Name.ReplaceRules(rules));
            }
            else
            {
                file.RenameIgnoreExtension(file.PureName.ReplaceRules(rules));
            }
        }
    }
}
