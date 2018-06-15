using Gihan.Renamer.Core.Models;
using Gihan.Renamer.Core.Models.Base;
using Gihan.Renamer.Models;
using System.Collections.Generic;

namespace Gihan.Renamer.Core
{
    public abstract class Renamer<FolderT, FileT, ItemT>
        where FolderT : IFolder where FileT : IFile where ItemT : IStorageItem
    {
        public Renamer()
        {

        }

        public abstract void RenameByRules(string dirPath, IEnumerable<RenameRule> renameRules);

        public void Start(FolderT folder)
        {
            folder.GetFolders();
        }
    }
    /*
    public class Renamer
    {
        private IStorageHelper StorageHelper { get; }

        public Renamer(IStorageHelper storageHelper = null)
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
    */
}
