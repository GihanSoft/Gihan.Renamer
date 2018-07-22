using Gihan.Renamer.Models;
using Gihan.Renamer.SystemIO;
using Xunit;

namespace TestSysIO
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var rules = new RenameRule[1];
            rules[0] = new RenameRule() { From = "n", To = "New " };
            var renamer = new Renamer(@"D:\WorkSpace\ws", rules);
            renamer.Start();
        }
    }
}
