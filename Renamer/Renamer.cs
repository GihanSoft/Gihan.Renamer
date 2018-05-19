using Gihan.Renamer.Models;
using Gihan.Renamer.Models.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gihan.Renamer
{
    public static class Renamer
    {
        private static string ReplaceRule(this string src, RenameRule rule)
        {
            var result = "";
            switch (rule.IsAlgo)
            {
                case false:
                    result = src.Replace(rule.From, rule.To);
                    break;
                case true:
                    result = src.ReplaceAlgo(rule.From, rule.To);
                    break;
            }
            return result;
        }
        private static string ReplaceRule(this string src, IEnumerable<RenameRule> rules)
        {
            var result = src;
            foreach (var rule in rules)
            {
                result = result.ReplaceRule(rule);
            }
            return result;
        }

        public static void Rename(this DirectoryInfo directory, IEnumerable<RenameRule> renameRules)
        {
            foreach (var dir in directory.EnumerateDirectories())
            {
                dir.Rename(renameRules);
            }
            var files = directory.EnumerateFiles();
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file.FullName);
                var destName = name.ReplaceRule(renameRules);
                file.Rename(destName);
            }
            var dirName = Path.GetFileNameWithoutExtension(directory.FullName);
            var destDirName = dirName.ReplaceRule(renameRules);
            directory.Rename(destDirName);
        }

        public static void Renme(string directoryPath, IEnumerable<RenameRule> renameRules)
        {
            var dir = new DirectoryInfo(directoryPath);
            if (!dir.Exists) throw new Exception();
            dir.Rename(renameRules);
        }

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

        static void Rename(this FileSystemInfo fileSystemInfo, string newName)
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

        static void RenameFt(this FileSystemInfo fileSystemInfo, string from, string to)
        {
            var name = Path.GetFileNameWithoutExtension(fileSystemInfo.FullName);
            var destName = name.Replace(from, to);
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

        static void RenameAlgo(this FileSystemInfo fileSystemInfo, string AlgoF, string AlgoTo)
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
