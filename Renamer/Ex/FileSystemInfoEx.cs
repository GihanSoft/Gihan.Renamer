using System;
using System.IO;
using Gihan.Renamer.Models.Enums;

namespace Gihan.Renamer.Ex
{
    internal static class FileSystemInfoEx
    {
        public static void MoveTo(this FileSystemInfo info, string dastName)
        {
            switch (info)
            {
                case FileInfo _:
                    (info as FileInfo)?.MoveTo(dastName);
                    break;
                case DirectoryInfo _:
                    (info as DirectoryInfo)?.MoveTo(dastName);
                    break;
                default:
                    throw new ArgumentException("unknown type of FileSystemInfo");
            }
        }

        public static void Rename(this FileSystemInfo fileSystemInfo,
            string newName, NameCollisionOption option)
        {
            var fullPath = fileSystemInfo.FullName;
            fullPath = fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var name = Path.GetFileNameWithoutExtension(fullPath);
            var index = fullPath.LastIndexOf(name, StringComparison.Ordinal);
            var destPath = fullPath.Replace(name, newName, index, name.Length);

            if (fullPath == destPath) return;

            if (!string.Equals(destPath, fullPath, StringComparison.OrdinalIgnoreCase))
                if (File.Exists(destPath) || Directory.Exists(destPath))
                {
                    switch (option)
                    {
                        case NameCollisionOption.GenerateUniqueName:
                            Rename(fileSystemInfo, newName + "_2", option);
                            return;
                        case NameCollisionOption.ReplaceExisting:
                            File.Delete(destPath);
                            if (File.Exists(destPath))
                                File.Delete(destPath);
                            else if (Directory.Exists(destPath))
                                Directory.Delete(destPath);
                            break;
                        case NameCollisionOption.FailIfExists:
                            throw new Exception("a File of Directory with same name is exist");
                        default:
                            throw new ArgumentOutOfRangeException(nameof(option), option, null);
                    }
                }

            try
            {
                fileSystemInfo.MoveTo(destPath);
            }
            catch (Exception)
            {


                if (!string.Equals(fullPath, destPath, StringComparison.CurrentCultureIgnoreCase)) throw;

                fileSystemInfo.MoveTo(fullPath + "_");
                fileSystemInfo.MoveTo(destPath);
            }
        }
    }
}
