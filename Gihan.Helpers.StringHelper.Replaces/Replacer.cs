using System;
using System.Collections.Generic;
using System.Linq;

namespace Gihan.Helpers.String
{
    public static class Replacer
    {
        public const char Joker = '*';
        public const char NumStartFlag = '<';
        public const char NumEndFlag = '>';

        private enum ReplaceType
        {
            Normal,
            Algo,
            NumericAlgo
        }

        private static string ReplaceNormal(this string src, Tuple<string, string> pattern)
        {
            //validations
            if (string.IsNullOrEmpty(pattern.Item1))
                throw new Exception("It's imposible replace nothing with any thing");

            return src.Replace(pattern.Item1, pattern.Item2);
        }

        private static string ReplaceAlgo(this string src,
                                          int jokerIndexFrom, /*int jokerIndexTo, */
                                          string fromPattern, string toPattern)
        {
            var algoFromParts = fromPattern.Split(Joker);
            var algoToParts = toPattern.Split(Joker);

            if (!src.StartsWith(algoFromParts.First()) || !src.EndsWith(algoFromParts.Last()))
                return src;

            var jEnd = src.LastIndexOf(algoFromParts.Last(), StringComparison.Ordinal);
            int jLength;
            if (algoFromParts.Last() != "")
                jLength = jEnd - jokerIndexFrom;
            else
                jLength = src.Length - jokerIndexFrom;

            var constPart = src.Substring(jokerIndexFrom, jLength);

            return algoToParts.First() + constPart + algoToParts.Last();
        }

        //--## numeric algo ##----------------------------------------------------------
        private static Tuple<string, string> _prePattern;
        private static int? _preNum;
        private static string _numFormat;

        public static void ResetNumeric()
        {
            _preNum = null;
            _numFormat = null;
        }

        private static string ReplaceNumericAlgo(this string src,
                                                 int numStartFlagIndex, int numEndFlagIndex,
                                                 string fromPattern, string toPattern)
        {
            var algoFromParts = fromPattern.Split(Joker);
            if (!src.StartsWith(algoFromParts.First()) || !src.EndsWith(algoFromParts.Last()))
                return src;

            if (fromPattern != _prePattern?.Item1 || toPattern != _prePattern?.Item2)
                ResetNumeric();

            if (_preNum is null || _numFormat is null)
            {
                var numLength = numEndFlagIndex - numStartFlagIndex - 1;
                var numPart = toPattern.Substring(numStartFlagIndex + 1, numLength);
                if (numPart.Any(ch => !char.IsDigit(ch)))
                    throw new Exception("you must put a integer number " +
                        $"between '{NumStartFlag}' and '{NumEndFlag}'");
                _preNum = int.Parse(numPart) - 1;
                _numFormat = "D" + numLength;
            }
            _prePattern = new Tuple<string, string>(fromPattern, toPattern);

            var before = toPattern.Split(NumStartFlag).First();
            var after = toPattern.Split(NumEndFlag).Last();

            return before + (++_preNum).Value.ToString(_numFormat) + after;
        }

        public static string Replace(this string src, Tuple<string, string> pattern)
        {
            ReplaceType replaceType;

            // find Joker ('*') in ``From``
            var jokerIndex = -1;
            for (var i = 0; i < pattern.Item1.Length; i++)
            {
                var ch = pattern.Item1[i];
                if (ch != Joker) continue;
                if (jokerIndex == -1) jokerIndex = i;
                else throw new Exception($"There is two '{Joker}' in From");
            }

            // find '*' or '<' and '>' in ``To``
            var numStartFlagIndex = -1;
            var numEndFlagIndex = -1;
            var joker2Index = -1;
            for (var i = 0; i < pattern.Item2.Length; i++)
            {
                var ch = pattern.Item2[i];
                switch (ch)
                {
                    case Joker:
                        if (joker2Index == -1) joker2Index = i;
                        else throw new Exception($"There is two '{Joker}' in From");
                        break;
                    case NumStartFlag:
                        if (numStartFlagIndex == -1) numStartFlagIndex = i;
                        else throw new Exception($"There is two '{NumStartFlag}' in From");
                        break;
                    case NumEndFlag:
                        if (numEndFlagIndex == -1) numEndFlagIndex = i;
                        else throw new Exception($"There is two '{NumEndFlag}' in From");
                        break;
                }
            }

            //set ``replaceType``
            if (jokerIndex == -1) replaceType = ReplaceType.Normal;
            else
            {
                if (joker2Index != -1) replaceType = ReplaceType.Algo;
                else if (numStartFlagIndex != -1 && numEndFlagIndex != -1)
                    replaceType = ReplaceType.NumericAlgo;
                else
                    throw new Exception("Unknown Replace Type.");
            }

            //validations
            switch (replaceType)
            {
                case ReplaceType.Normal:
                    if (numStartFlagIndex != -1)
                        throw new Exception($"There is a '{numStartFlagIndex}' in Normal replace");
                    if (numEndFlagIndex != -1)
                        throw new Exception($"There is a '{numEndFlagIndex}' in Normal replace");
                    if (joker2Index != -1)
                        throw new Exception($"There is a '{Joker}' in Normal replace");
                    //imposible
                    if (jokerIndex != -1)
                        throw new Exception($"There is a '{Joker}' in Normal replace");
                    break;
                case ReplaceType.Algo:
                    if (numStartFlagIndex != -1)
                        throw new Exception($"There is a '{numStartFlagIndex}' in Algi replace");
                    if (numEndFlagIndex != -1)
                        throw new Exception($"There is a '{numEndFlagIndex}' in Algo replace");
                    //imposible
                    if (jokerIndex == -1)
                        throw new Exception($"There is no '{Joker}' in 'from' of Algo replace");
                    if (joker2Index == -1)
                        throw new Exception($"There is no '{Joker}' in 'to' of Algo replace");
                    break;
                case ReplaceType.NumericAlgo:
                    if (numStartFlagIndex > numEndFlagIndex)
                        throw new Exception($"'{NumStartFlag}' must be after '{NumEndFlag}'");
                    //imposible
                    if (numStartFlagIndex == -1)
                        throw new Exception($"There is no '{numStartFlagIndex}' in NumericAlgi replace");
                    if (numEndFlagIndex == -1)
                        throw new Exception($"There is no '{numEndFlagIndex}' in NumericAlgo replace");
                    if (jokerIndex == -1)
                        throw new Exception($"There is no '{Joker}' in 'from' of NumericAlgo replace");
                    if (joker2Index != -1)
                        throw new Exception($"There is a '{Joker}' in 'to' of NumericAlgo replace");
                    break;
                default:
                    throw new Exception(@":\");
            }

            switch (replaceType)
            {
                case ReplaceType.Normal:
                    return ReplaceNormal(src, pattern);
                case ReplaceType.Algo:
                    return ReplaceAlgo(src, jokerIndex, /*joker2Index,*/ pattern.Item1, pattern.Item2);
                case ReplaceType.NumericAlgo:
                    return ReplaceNumericAlgo(src, numStartFlagIndex, numEndFlagIndex,
                                              pattern.Item1, pattern.Item2);
                default:
                    throw new Exception(@":\");
            }
        }
        public static string Replaces(this string src, IEnumerable<Tuple<string, string>> patterns)
        {
            return patterns.Aggregate(src, (current, pattern) => current.Replace(pattern));
        }
    }
}
