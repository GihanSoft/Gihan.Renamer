using System;
using System.Collections.Generic;
using System.Linq;
using Gihan.Renamer.Models;

namespace Gihan.Renamer.Ex
{
    public static class StringEx
    {
        public static string ReplaceAlgo(this string src, string algoF, string algoTo)
        {
            var algoFParts = algoF.Split('*');
            var algoToParts = algoTo.Split('*');

            if (algoFParts.Length != 2)
                throw new ArgumentException("More than one '*' is in To Algo", nameof(algoF));
            if (algoToParts.Length != 2)
                throw new ArgumentException("More than one '*' is in From Algo", nameof(algoTo));

            if (!src.StartsWith(algoFParts[0]) || !src.EndsWith(algoFParts[1]))
                return src;

            var jIndex = algoF.IndexOf("*", StringComparison.Ordinal);
            var jEnd = src.LastIndexOf(algoFParts[1], StringComparison.Ordinal);
            int jLength;
            if (algoFParts[1] != "")
                jLength = jEnd - jIndex;
            else
                jLength = src.Length - jIndex;

            var constPart = src.Substring(jIndex, jLength);

            return algoToParts[0] + constPart + algoToParts[1];
        }

        public static string ReplaceRule(this string src, RenameRule rule)
        {
            var isAlgo = false;

            var fromContainsJoker = rule.From.Contains("*");
            var toContainsJoker = rule.From.Contains("*");
            if (fromContainsJoker && toContainsJoker)
            {
                isAlgo = true;
            }
            else
            {
                if (fromContainsJoker || toContainsJoker)
                {
                    throw new Exception("For an algo rename, both of 'from' & 'to' must have one '*'.");
                }
            }

            var result = "";
            if (!isAlgo)
            {
                if (string.IsNullOrEmpty(rule.From))
                    throw new Exception("Cannot Replace nothing with any thing");
                result = src.Replace(rule.From, rule.To);
            }
            else
            {
                result = src.ReplaceAlgo(rule.From, rule.To);
            }

            return result;
        }

        public static string ReplaceRules(this string src, IEnumerable<RenameRule> rules)
        {
            return rules.Aggregate(src, (current, rule) => current.ReplaceRule(rule));
        }

        //public static string Replace(this string src, string oldValue, string newValue,
        //    int startIndex, int length)
        //{
        //    var s1 = src.Substring(0, startIndex);
        //    var s2 = src.Substring(startIndex, length);
        //    var s3 = src.Substring(startIndex + length);

        //    s2 = s2.Replace(oldValue, newValue);
        //    return s1 + s2 + s3;
        //}
    }
}