using System;

namespace Gihan.Renamer.Models
{
    [Flags]
    public enum RenameFlags : uint
    {
        Zero = 0b0_0000,
        Extension = 0b0_0001,
        Folder = 0b0_0010,
        SubFolders = 0b0_0100,

        Default = 0b0_0110,
    }
}