using Gihan.Renamer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var rulesArray = renameRules as RenameRule[] ?? renameRules.ToArray();
            foreach (var dir in directory.GetDirectories())
            {
                dir.Rename(rulesArray);
            }
            var files = directory.GetFileSystemInfos();
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file.FullName);
                var destName = name.ReplaceRule(rulesArray);
                file.Rename(destName);
            }
        }

        public static void Rename(string directoryPath, IEnumerable<RenameRule> renameRules)
        {
            var dir = new DirectoryInfo(directoryPath);
            if (!dir.Exists) throw new Exception();
            dir.Rename(renameRules);
        }

        #region Extension

        private static void MoveTo(this FileSystemInfo info, string dastName)
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

        private static string Replace(this string src, string oldValue, string newValue, int startIndex, int length)
        {
            var s1 = src.Substring(0, startIndex);
            var s2 = src.Substring(startIndex, length);
            var s3 = src.Substring(startIndex + length);

            s2 = s2.Replace(oldValue, newValue);
            return s1 + s2 + s3;
        }

        private static void Rename(this FileSystemInfo fileSystemInfo, string newName)
        {
            var fullPath = fileSystemInfo.FullName;
            fullPath = fullPath.TrimEnd('\\');
            var name = Path.GetFileNameWithoutExtension(fullPath);
            var index = fullPath.LastIndexOf(name, StringComparison.Ordinal);
            var destPath = fullPath.Replace(name, newName, index, name.Length);
            try
            {
                fileSystemInfo.MoveTo(destPath);
            }
            catch (Exception)
            {
                if (fullPath == destPath) return;
                if (!string.Equals(fullPath, destPath, StringComparison.CurrentCultureIgnoreCase)) throw;
                fileSystemInfo.MoveTo(fullPath + "_");
                fileSystemInfo.MoveTo(destPath);
            }
        }

        private static void RenameFt(this FileSystemInfo fileSystemInfo, string from, string to)
        {
            var name = Path.GetFileNameWithoutExtension(fileSystemInfo.FullName);
            var destName = name.Replace(from, to);
            fileSystemInfo.Rename(destName);
        }

        private static string ReplaceAlgo(this string src, string algoF, string algoTo)
        {
            var algoFParts = algoF.Split('*');
            var algoToParts = algoTo.Split('*');

            if (algoToParts.Length > 2) throw new Exception();
            if (algoFParts.Length > 2) throw new Exception();

            if (!src.StartsWith(algoFParts[0]) || !src.EndsWith(algoFParts[1]))
                return src;

            var jIndex = algoF.IndexOf("*", StringComparison.Ordinal);
            var jEnd = src.LastIndexOf(algoFParts[1], StringComparison.Ordinal);
            var jLength = jEnd - jIndex;
            if (algoFParts[1] == "") jLength++;
            var constPart = src.Substring(jIndex, jLength);

            return algoToParts[0] + constPart + algoToParts[1];
        }

        private static void RenameAlgo(this FileSystemInfo fileSystemInfo, string AlgoF, string AlgoTo)
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
