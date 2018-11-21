using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gihan.Helpers.String;
using Gihan.Renamer.Models;
using Gihan.Renamer.Models.Enums;
using Gihan.Storage.Core;
using Gihan.Storage.Core.Base;

namespace Gihan.Renamer
{
    public abstract class RenameProcessor
    {
        protected abstract StorageHelper StorageHelper { get; }

        protected static IEnumerable<IStorageItem> FetchItems(IFolder rootFolder, RenameFlags renameFlags)
        {
            var items = new List<IStorageItem>();

            const RenameFlags arefoldersUsed = RenameFlags.Folder | RenameFlags.SubFolders;
            IFolder[] folders = null;
            if ((arefoldersUsed & renameFlags) > 0)
            {
                folders = rootFolder.GetFolders().ToArray();
            }

            if (renameFlags.HasFlag(RenameFlags.Files))
            {
                items.AddRange(rootFolder.GetFiles());
            }

            if (folders is null) return items;
            if (renameFlags.HasFlag(RenameFlags.Folder))
            {
                items.AddRange(folders);
            }

            if (!renameFlags.HasFlag(RenameFlags.SubFolders)) return items;
            foreach (var folder in folders)
            {
                items.AddRange(FetchItems(folder, renameFlags));
            }
            return items;
        }

        public IEnumerable<RenameOrder> ProcessReplace
            (IEnumerable<IStorageItem> items, IEnumerable<ReplacePattern> patterns, RenameFlags renameFlags)
        {
            if (!(items is List<IStorageItem>))
                items = items.ToList();
            (items as List<IStorageItem>).OrderByDescending(i => i.Path, NaturalStringComparer.Default);
            var orderList = new List<RenameOrder>();
            var patternsArr = patterns.ToArray();
            foreach (var storageItem in items)
            {
                var order = new RenameOrder { FilePath = storageItem.Path };
                try
                {
                    if (!storageItem.Exist)
                        throw new Exception($"'{storageItem.Path}' not exist");

                    var useExtension = !(storageItem is IFile) || renameFlags.HasFlag(RenameFlags.Extension);
                    var name = useExtension ? storageItem.Name : (storageItem as IFile).PureName;
                    var destName = name.Replaces(patternsArr);
                    order.NewName = destName;

                    var destPath = Path.Combine(storageItem.Parent.Path, destName);
                    if (StorageHelper.Creat().Exist(destPath))
                        throw new Exception($"A file with same path '{destPath}' already exist");
                    if (orderList.Any(o => Path.Combine(Path.GetDirectoryName(o.FilePath), o.NewName) == destPath))
                        throw new Exception($"Some other file will rename to {destPath}");
                }
                catch (Exception err)
                {
                    order.Message = $"{err.GetType().FullName}: {err.Message}";
                }
                orderList.Add(order);
            }
            return orderList.OrderBy(o => o.FilePath, NaturalStringComparer.Default);
        }

        public IEnumerable<RenameOrder> ProcessReplace
            (IFolder rootFolder, IEnumerable<ReplacePattern> patterns, RenameFlags renameFlags)
        {
            var items = FetchItems(rootFolder, renameFlags);
            return ProcessReplace(items, patterns, renameFlags);
        }
    }
}
