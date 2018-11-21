using System;

namespace Gihan.Renamer.Models.Enums
{
    [Flags]
    public enum RenameFlags : byte
    {
        Zero = /********/ 0b0_0000,
        /// <summary>
        /// inclide extension in rename
        /// </summary>
        Extension = /***/ 0b0_0001,
        /// <summary>
        /// include files in rename
        /// </summary>
        Files = /*******/ 0b0_0010,
        /// <summary>
        /// include folders in rename
        /// </summary>
        Folder = /******/ 0b0_0100,
        /// <summary>
        /// include sub folders in rename
        /// </summary>
        SubFolders = /**/ 0b0_1000,
        /// <summary>
        /// include items in subfolders and folders but not extensions : 
        /// <see cref="Folder"/> | <see cref="SubFolders"/>
        /// </summary>
        Default = /*****/ 0b0_1110,
    }
}