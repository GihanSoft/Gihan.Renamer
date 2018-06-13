using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gihan.Renamer.Core;
using Gihan.Renamer.Core.Base;
using Gihan.Renamer.Core.Enums;
using Gihan.Renamer.SystemIO.Base;

using SysPath = System.IO.Path;
using SysIo = System.IO;

namespace Gihan.Renamer.SystemIO
{
    public class Folder : StorageItem, IFolder
    {
        private Folder _parent;

        protected DirectoryInfo BaseFolder => (DirectoryInfo)BaseStorageItem;

        public override IFolder Parent => _parent ?? (_parent = new Folder(BaseFolder.Parent));

        public override StorageItemType Type => StorageItemType.Folder;

        public Folder(DirectoryInfo item) : base(item)
        {
            if (!item.Exists)
                throw new ArgumentException("Folder is not exist", nameof(item));
        }

        public Folder(string folderPath) : base(new DirectoryInfo(folderPath))
        {
            if (!Directory.Exists(folderPath))
                throw new ArgumentException("Folder is not exist", nameof(folderPath));
        }

        public IReadOnlyList<IFile> GetFiles(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var baseFiles = BaseFolder.EnumerateFiles("*.*", option);
            var files = baseFiles.Select(bf => new File(bf));
            return files.ToList().AsReadOnly();
        }

        public IReadOnlyList<IFolder> GetFolders(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var baseFolders = BaseFolder.EnumerateDirectories("*", option);
            var folders = baseFolders.Select(bf => new Folder(bf));
            return folders.ToList().AsReadOnly();
        }

        public IReadOnlyList<IStorageItem> GetItems(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var baseFiles = BaseFolder.EnumerateFiles("*.*", option);
            var baseFolders = BaseFolder.EnumerateDirectories("*", option);
            var items =
                baseFiles.Select<FileInfo, StorageItem>(bf => new File(bf)).
                Concat(baseFolders.Select<DirectoryInfo, StorageItem>(bf => new Folder(bf)));
            return items.ToList().AsReadOnly();
        }

        public override void Rename(string desiredName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            var destFullPath = SysPath.Combine(Parent.Path, desiredName);

            StorageItem item = null;
            if (Directory.Exists(destFullPath))
            {
                item = new Folder(destFullPath);
            }
            else if (SysIo.File.Exists(destFullPath))
            {
                item = new File(destFullPath);
            }

            if (item != null)
            {
                switch (option)
                {
                    case NameCollisionOption.GenerateUniqueName:
                        destFullPath = NextName(destFullPath);
                        break;
                    case NameCollisionOption.ReplaceExisting:
                        item.Delete();
                        break;
                    case NameCollisionOption.FailIfExists:
                        // System.IO default option. .net will throw exception
                        break;
                    default:
                        throw new ArgumentException("invalid option", nameof(option));
                }
            }
            BaseFolder.MoveTo(destFullPath);
        }
    }
}
