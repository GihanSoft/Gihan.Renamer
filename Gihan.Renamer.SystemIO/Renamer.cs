using System;
using System.Collections.Generic;
using System.Text;
using Gihan.Renamer.Models;

namespace Gihan.Renamer.SystemIO
{
    class Renamer : Core.Renamer<Folder, File, Base.StorageItem>
    {
        public override void RenameByRules(string dirPath, IEnumerable<RenameRule> renameRules)
        {
            Start(new Folder(dirPath));
        }
    }
}
