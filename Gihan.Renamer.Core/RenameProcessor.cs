using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gihan.Helpers.Linq;
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
            if (renameFlags.HasFlag(RenameFlags.Files))
                items.AddRange(rootFolder.GetFiles());

            IEnumerable<IFolder> folders = null;
            if (renameFlags.HasFlag(RenameFlags.Folder))
                items.AddRange(folders ?? (folders = rootFolder.GetFolders()));
            if (renameFlags.HasFlag(RenameFlags.SubFolders))
            {
                foreach (var folder in folders ?? rootFolder.GetFolders())
                {
                    items.AddRange(FetchItems(folder, renameFlags));
                }
            }
            return items;
        }

        protected IEnumerable<RenameOrder> ProcessReplace
            (IEnumerable<IStorageItem> items, IEnumerable<ReplacePattern> patterns, 
            RenameFlags renameFlags, string rootFolder = null)
        {
            items = items.GroupBy(i => i.Parent.Path).NaturalOrderByDescending(g => g.Key).
                     SelectMany(g => g.NaturalOrderBy(i => i.Path));

            var orderList = new List<RenameOrder>();
            foreach (var storageItem in items)
            {
                var order = new RenameOrder { Path = storageItem.Path };
                try
                {
                    if (!storageItem.Exist)
                        throw new Exception($"'{storageItem.Path}' not exist");

                    var usePurename = (storageItem is IFile fileItem) && !renameFlags.HasFlag(RenameFlags.Extension);
                    var name = usePurename ? (storageItem as IFile).PureName : storageItem.Name;
                    var destName = name.Replaces(patterns) + (usePurename ? (storageItem as IFile).Extension : "");
                    var destPath = Path.Combine(storageItem.Parent.Path, destName);

                    order.DestPath = destPath;

                    if (StorageHelper.Exist(destPath))
                        throw new Exception($"A file with same path '{destPath}' already exist");
                    if (orderList.Any(o => string.Equals(o.DestPath, destPath, StringComparison.OrdinalIgnoreCase)))
                        throw new Exception($"Some other file will rename to {destPath}");
                }
                catch (Exception err)
                {
                    order.Message = $"{err.GetType().FullName}: {err.Message}";
                }
                orderList.Add(order);
            }

            using (var db = new RenameDbContext())
            {
                db.Processes.Insert(new RenameProcess()
                {
                    DateTime = DateTime.Now,
                    RootFolder = rootFolder,
                    Items = items.Select(i => i.Path),
                    Patterns = patterns,
                    RenameFlags = renameFlags
                });
            }

            return orderList;
        }

        public IEnumerable<RenameOrder> ProcessReplace
            (IEnumerable<IStorageItem> items, IEnumerable<ReplacePattern> patterns, RenameFlags renameFlags)
                => ProcessReplace(items, patterns, renameFlags);

        public IEnumerable<RenameOrder> ProcessReplace
            (IFolder rootFolder, IEnumerable<ReplacePattern> patterns, RenameFlags renameFlags)
        {
            var items = FetchItems(rootFolder, renameFlags);
            return ProcessReplace(items, patterns, renameFlags, rootFolder.Path);
        }
    }
}
