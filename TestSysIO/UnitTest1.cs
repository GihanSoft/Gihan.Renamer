using Gihan.Helpers.String;
using Gihan.Renamer.Models;
using Gihan.Renamer.Models.Enums;
using Gihan.Renamer.SystemIO;
using System.Linq;
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

        [Fact]
        public void Test2()
        {
            var renameProcessor = new RenameProcessor();
            var orders = renameProcessor.ProcessReplace(@"D:\WorkSpace\rename",
                new ReplacePattern[] { ("(", "["), (")", "]") }, RenameFlags.Default);
            var renamer = new Renamer();
            renamer.Rename(orders);
        }

        [Fact]
        public void Test3()
        {
            var src = "some text";
            var dest1 = src.Replace(("not*", "text<01>"));
            Assert.Equal(src, dest1);
            var dest2 = src.Replace(("some*", "text<-1>"));
            var re = "me".Replace(("text-1", dest2));
        }
    }
}
