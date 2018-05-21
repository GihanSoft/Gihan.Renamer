using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gihan.Renamer.Ex;
using Gihan.Renamer.Models.Enums;

namespace Gihan.Renamer
{
    public class StorageHelperSysIo : Base.IStorageHelper
    {

        public void RenameWithoutExtension(string targetPath, string destName, 
            NameCollisionOption option = NameCollisionOption.GenerateUniqueName)
        {
            targetPath = targetPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var attr = File.GetAttributes(targetPath);
            FileSystemInfo fileSystemInfo;
            if (attr.HasFlag(FileAttributes.Directory))
                fileSystemInfo = new DirectoryInfo(targetPath);
            else
                fileSystemInfo = new FileInfo(targetPath);
            fileSystemInfo.Rename(destName, option);
        }

        public void Rename(string targetPath, string destName, 
            NameCollisionOption option = NameCollisionOption.GenerateUniqueName)
        {
            targetPath = targetPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var extension = Path.GetExtension(targetPath);
            RenameWithoutExtension(targetPath, destName + extension, option);
        }

        public IEnumerable<string> GetSubFolders(string dirPath)
        {
            var dir = new DirectoryInfo(dirPath);
            var subDirsPath = dir.EnumerateDirectories().Select(d => d.FullName);
            return subDirsPath;
        }

        public IEnumerable<string> GetSubItems(string dirPath)
        {
            var dir = new DirectoryInfo(dirPath);
            var subItemsPath = dir.EnumerateFileSystemInfos().Select(i => i.FullName);
            return subItemsPath;
        }
    }
}
