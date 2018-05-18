using Gihan.Renamer.Models.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gihan.Renamer
{
    public static class Renamer
    {


        #region Extension
        static void MoveTo(this FileSystemInfo info, string dastName)
        {
            if (info is FileInfo) (info as FileInfo).MoveTo(dastName);
            else
            if (info is DirectoryInfo) (info as DirectoryInfo).MoveTo(dastName);
            else
                throw new ArgumentException("unknown type of FileSystemInfo");
        }

        static string Replace(this string src, string oldValue, string newValue, int startIndex, int length)
        {
            string s1 = src.Substring(0, startIndex);
            string s2 = src.Substring(startIndex, length);
            string s3 = src.Substring(startIndex + length);

            s2 = s2.Replace(oldValue, newValue);
            return s1 + s2 + s3;
        }

        public static void Rename(this FileSystemInfo fileSystemInfo, string newName)
        {
            var fullPath = fileSystemInfo.FullName;
            fullPath = fullPath.EndsWith("\\") ? fullPath.Substring(0, fullPath.Length - 1) : fullPath;
            var name = Path.GetFileNameWithoutExtension(fullPath);
            var index = fullPath.LastIndexOf(name);
            var destPath = fullPath.Replace(name, newName, index, name.Length);
            fileSystemInfo.MoveTo(destPath);
        }

        static string ReplaceFt(this string src, IEnumerable<Tuple<string, string>> fts)
        {
            foreach (var ft in fts)
            {
                src = src.Replace(ft.Item1, ft.Item2);
            }
            return src;
        }

        public static void RenameFt(this FileSystemInfo fileSystemInfo, IEnumerable<Tuple<string, string>> fts)
        {
            var name = Path.GetFileNameWithoutExtension(fileSystemInfo.FullName);
            var destName = name.ReplaceFt(fts);
            try
            {
                fileSystemInfo.Rename(destName);
            }
            catch (Exception err)
            {
                if (name == destName) return;
                if (name.ToLower() == destName.ToLower())
                {
                    fileSystemInfo.Rename("_" + name);
                    fileSystemInfo.Rename(destName);
                    return;
                }
                throw err;
            }
        }

        public static void RenameAlgo(this FileSystemInfo fileSystemInfo, string AlgoF, string AlgoTo)
        {

        }

        #endregion
    }
}
