namespace Gihan.Renamer.Models
{
    public class RenameRule
    {
        public string From { get; set; }
        public string To { get; set; }
        public bool IsAlgo { get; set; }
        public RenameRule() { }
        public RenameRule(string from, string to, bool isAlgo = false)
        {
            From = from;
            To = to;
            IsAlgo = isAlgo;
        }
    }
}
