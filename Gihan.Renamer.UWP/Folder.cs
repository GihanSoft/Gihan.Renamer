using Gihan.Renamer.Core;
using System;
using System.Collections.Generic;
using Windows.Storage;

namespace Gihan.Renamer.UWP
{
    public class Folder : Base.StorageItem, IFolder
    {
        StorageFolder BaseFolder { get; }

        public IReadOnlyList<IFile> GetFiles()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IFolder> GetFolders()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Core.Base.IStorageItem> GetItems()
        {
            throw new NotImplementedException();
        }
    }
}
