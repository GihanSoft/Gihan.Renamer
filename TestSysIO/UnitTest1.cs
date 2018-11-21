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
            var x = new RenameProcessor();
            var rootFolderPath = @"D:\WorkSpace\New folder";
            
           var rr = x.ProcessReplace(rootFolderPath, new[] { new ReplacePattern { From = "t", To = "h" } }, RenameFlags.Default);

            var y = new Renamer();
            y.Rename(rr, RenameFlags.Default);
            var db = new Db();
            var arr = db.Renames.ToArray();
        }
    }
}
