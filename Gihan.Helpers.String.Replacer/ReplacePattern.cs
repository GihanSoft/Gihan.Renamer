using System;

namespace Gihan.Helpers.String
{
    public class ReplacePattern
    {
        public string From { get; set; }
        public string To { get; set; }

        public override string ToString()
        {
            return $"\"{From}\" => \"{To}\"";
        }

        #region Cast
        public static implicit operator ReplacePattern(Tuple<string, string> tuple)
        {
            return new ReplacePattern { From = tuple.Item1, To = tuple.Item2 };
        }

        public static implicit operator ReplacePattern((string, string) tuple)
        {
            return new ReplacePattern { From = tuple.Item1, To = tuple.Item2 };
        }
        #endregion
    }
}