using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gihan.Renamer.Base;
using Gihan.Renamer.Ex;
using Gihan.Renamer.Models;

namespace Gihan.Renamer
{
    public class Renamer
    {
        private IStorageHelper StorageHelper { get; }

        public Renamer(IStorageHelper storageHelper)
        {
            StorageHelper = storageHelper?? new StorageHelperSysIo();
        }

        public void RenameByRules(string dirPath, IEnumerable<RenameRule> renameRules)
        {
            var rulesArray = renameRules as RenameRule[] ?? renameRules.ToArray();
            var subFolders = StorageHelper.GetSubFolders(dirPath);
            foreach (var subFolder in subFolders)
            {
                RenameByRules(subFolder, rulesArray);
            }
            var items = StorageHelper.GetSubItems(dirPath);
            foreach (var item in items)
            {
                var name = Path.GetFileNameWithoutExtension(item);
                var destName = name.ReplaceRules(rulesArray);
                StorageHelper.RenameWithoutExtension(item, destName);
            }
        }
    }
}
