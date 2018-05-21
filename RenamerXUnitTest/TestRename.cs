using Xunit;
using Gihan.Renamer;
using Gihan.Renamer.Models;

namespace RenamerXUnitTest
{
    public class TestRename
    {
        [Fact]
        public void X()
        {
            Renamer renamer = new Renamer();
            var rules = new[]
            {
                new RenameRule("BTOOOM.", "Btooom."),
            };
            renamer.RenameByRules(@"E:\Entertainment\Anime\Btooom!", rules);
        }
    }
}
