using System;
using System.IO;
using Xunit;
using Gihan.Renamer;
using System.Collections.Generic;

namespace RenamerXUnitTest
{
    public class TestRename
    {
        [Fact]
        public void TestRenameFile()
        {
            var dest = new FileInfo(@"D:\WorkSpace\c.rar");
            Assert.False(dest.Exists);
            Renamer.Rename(new FileInfo(@"D:\WorkSpace\b.rar"), "c");
            dest = new FileInfo(@"D:\WorkSpace\c.rar");
            Assert.True(dest.Exists);
            dest.Rename("b");
        }

        [Fact]
        public void TestRenameDir()
        {
            var dest = new DirectoryInfo(@"D:\WorkSpace\c");
            Assert.False(dest.Exists);
            Renamer.Rename(new DirectoryInfo(@"D:\WorkSpace\b"), "c");
            dest = new DirectoryInfo(@"D:\WorkSpace\c");
            Assert.True(dest.Exists);
            dest.Rename("b");
        }

        [Fact]
        public void TestRenameFt()
        {
            var src = new DirectoryInfo(@"D:\WorkSpace\b");
            var fts = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("b", "c"),
                new Tuple<string, string>("c", "d")
            };
            src.RenameFt(fts);
            var dest = new DirectoryInfo(@"D:\WorkSpace\d");
            Assert.True(dest.Exists);
            dest.RenameFt(new[] { new Tuple<string, string>("d", "b") });
        }

        [Fact]
        public void TestRenameLowerUpperFt()
        {
            var src = new DirectoryInfo(@"D:\WorkSpace\b");
            var fts = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("b", "B")
            };
            src.RenameFt(fts);
            var dest = new DirectoryInfo(@"D:\WorkSpace\B");
            Assert.True(dest.Exists);
            dest.RenameFt(new[] { new Tuple<string, string>("B", "b") });
        }

        [Fact]
        public void RenameAlgoTest()
        {
            var src = new DirectoryInfo(@"D:\WorkSpace\ab123cd");
            src.RenameAlgo("ab*cd", "*");
            src.RenameAlgo("*", "ab*cd");
            src.RenameAlgo("ab*cd", "*cd");
            src.RenameAlgo("*", "ab*");
        }
    }
}
