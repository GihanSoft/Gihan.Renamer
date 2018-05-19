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
            fullPath = fullPath.TrimEnd('\\');
            var name = Path.GetFileNameWithoutExtension(fullPath);
            var index = fullPath.LastIndexOf(name);
            var destPath = fullPath.Replace(name, newName, index, name.Length);
            try
            {
                fileSystemInfo.MoveTo(destPath);
            }
            catch (Exception err)
            {
                if (fullPath == destPath) return;
                if (fullPath.ToLower() == destPath.ToLower())
                {
                    fileSystemInfo.MoveTo(fullPath + "_");
                    fileSystemInfo.MoveTo(destPath);
                    return;
                }
                throw err;
            }
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
            fileSystemInfo.Rename(destName);
        }

        static string ReplaceAlgo(this string src, string AlgoF, string AlgoTo)
        {
            var AlgoFParts = AlgoF.Split('*');
            var AlgoToParts = AlgoTo.Split('*');

            if (AlgoToParts.Length > 2) throw new Exception();
            if (AlgoFParts.Length > 2) throw new Exception();

            var jIndex = AlgoF.IndexOf("*");
            var jEnd = src.LastIndexOf(AlgoFParts[1]);
            var jLength = jEnd - jIndex;
            if (AlgoFParts[1] == "") jLength++;
            string constPart = src.Substring(jIndex, jLength);

            return AlgoToParts[0] + constPart + AlgoToParts[1];
        }

        public static void RenameAlgo(this FileSystemInfo fileSystemInfo, string AlgoF, string AlgoTo)
        {
            var fullPath = fileSystemInfo.FullName;
            fullPath = fullPath.TrimEnd('\\');
            var name = Path.GetFileNameWithoutExtension(fullPath);
            var destName = name.ReplaceAlgo(AlgoF, AlgoTo);

            fileSystemInfo.Rename(destName);
        }

        #endregion
    }
}
