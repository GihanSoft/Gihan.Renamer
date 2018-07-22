using System;

namespace Gihan.Renamer.Models
{
    public class RenameRule //: IEquatable<RenameRule>
    {
        public string From { get; set; } = null;
        public string To { get; set; } = null;

        public Tuple<string, string> ToTuple()
        {
            return new Tuple<string, string>(From, To);
        } 

        public override string ToString()
        {
            return $"\"{From}\" => \"{To}\"";
        }


        //public bool Equals(RenameRule other)
        //{
        //    if (other is null) return false;
        //    if (ReferenceEquals(this, other)) return true;
        //    return string.Equals(From, other.From) && string.Equals(To, other.To);
        //}
    }
}
