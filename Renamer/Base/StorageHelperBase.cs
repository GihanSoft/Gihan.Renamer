using System.Collections.Generic;
using Gihan.Renamer.Models.Enums;

namespace Gihan.Renamer.Base
{
    public abstract class StorageHelperBase
    {
        public abstract void RenameWithoutExtension(string targetPath, string destName,
            NameCollisionOption option = NameCollisionOption.GenerateUniqueName);
        public abstract void Rename(string targetPath, string destName,
            NameCollisionOption option = NameCollisionOption.GenerateUniqueName);

        public abstract IEnumerable<string> GetSubFolders(string dirPath);
        public abstract IEnumerable<string> GetSubItems(string dirPath);
    }
}
