using System.Collections.Generic;
using Gihan.Renamer.Models.Enums;

namespace Gihan.Renamer.Base
{
    public interface IStorageHelper
    {
        void RenameWithoutExtension(string targetPath, string destName,
            NameCollisionOption option = NameCollisionOption.GenerateUniqueName);
        void Rename(string targetPath, string destName,
            NameCollisionOption option = NameCollisionOption.GenerateUniqueName);

        IEnumerable<string> GetSubFolders(string dirPath);
        IEnumerable<string> GetSubItems(string dirPath);
    }
}
