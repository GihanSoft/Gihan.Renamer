using System;
using System.Collections.Generic;
using System.Text;

namespace Gihan.Renamer.Models.Enum
{
    [Flags]
    public enum RenameMode : byte
    {
        File = 1,
        Folder = 2,
        FileAndFolder = 3
    }
}
