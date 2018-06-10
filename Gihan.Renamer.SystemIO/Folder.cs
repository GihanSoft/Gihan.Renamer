using System;
using System.Collections.Generic;
using Gihan.Renamer.Core;
using Gihan.Renamer.Core.Base;

namespace Gihan.Renamer.SystemIO
{
    public class Folder : Base.StorageItem, IFolder
    {
        public IReadOnlyList<IFile> GetFiles()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IFolder> GetFolders()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IStorageItem> GetItems()
        {
            throw new NotImplementedException();
        }
    }
}
