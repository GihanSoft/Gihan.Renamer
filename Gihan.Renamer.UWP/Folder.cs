using Gihan.Renamer.Core;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace Gihan.Renamer.UWP
{
    public class Folder : Base.StorageItem, IFolder
    {
        private StorageFolder BaseFolder { get; }

        public IReadOnlyList<IFile> GetFiles(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IFolder> GetFolders(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Core.Base.IStorageItem> GetItems(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            throw new NotImplementedException();
        }
    }
}
