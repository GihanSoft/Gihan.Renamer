using Gihan.Helpers.String;
using Gihan.Renamer.Models;
using Gihan.Renamer.Models.Enums;
using Gihan.Renamer.SystemIO;
using Xunit;

namespace TestSysIO
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var src = "this is a text";
            src = src.Replaces(new[] { ("this", "This"), ("*text", "*TEXT") });
            Assert.Equal("This is a TEXT", src);
            src = src.Replace(("*", "This Is <001> TEXT"));
            src = src.Replace(("*", "This Is <001> TEXT"));
            src = src.Replace(("*", "This Is <001> TEXT"));
            Assert.Equal("This Is 003 TEXT", src);
            src = src.Replace(("*", "This Is <-05> Text"));
            src = src.Replace(("*", "This Is <-05> Text"));
            src = src.Replace(("*", "This Is <-05> Text"));
            Assert.Equal("This Is -03 Text", src);
        }
    }
}
