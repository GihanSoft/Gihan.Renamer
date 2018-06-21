using Gihan.Storage.Core;
using Gihan.Storage.Core.Base;
using Gihan.Renamer.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Gihan.Storage.Core.Enums;
using Gihan.Renamer.Ex;
using System.Threading;

namespace Gihan.Renamer
{
    public class Renamer
    {
        private bool WhileIsRunning { get; set; }
        private bool Analized { get; set; }
        private List<RenameLog> InnerLogList { get; set; }
        protected IFolder RootFolder { get; private set; }
        protected List<IStorageItem> ItemsToRename { get; private set; }
        protected RenameRule[] RenameRules { get; private set; }
        public bool Paused { get; protected set; }

        public bool IncludeExtension { get; }
        public IReadOnlyList<RenameLogReadOnly> LogList =>
                InnerLogList.Cast<RenameLogReadOnly>().ToList().AsReadOnly();
        public int RenamedCount => InnerLogList.Count;
        public int TrueRenamedCount
        {
            get
            {
                int count = 0;
                foreach (var item in InnerLogList)
                {
                    if (item.Before != item.After) count++;
                }
                return count;
            }
        }

        public Renamer(IFolder folder, IEnumerable<RenameRule> renameRules, bool includeExtension = false)
        {
            WhileIsRunning = false;
            Analized = false;
            RootFolder = folder;
            Paused = false;
            IncludeExtension = includeExtension;
            RenameRules = renameRules.ToArray();
            InnerLogList = new List<RenameLog>();
            ItemsToRename = new List<IStorageItem>();
        }

        private async Task Analize(IFolder folder)
        {
            await Task.Run(async () =>
            {
                ItemsToRename.AddRange(folder.GetFiles());
                foreach (var item in folder.GetFolders())
                {
                    await Analize(item);
                }
                ItemsToRename.Add(folder);
            });
        }

        private async Task Rename()
        {
            await Task.Run(() =>
            {
                WhileIsRunning = true;
                while (RenamedCount < ItemsToRename.Count)
                {
                    if (Paused) break;
                    var itemToRename = ItemsToRename[RenamedCount];
                    var destName = itemToRename.Name;
                    if (itemToRename.Type == StorageItemType.File && !IncludeExtension)
                    {
                        destName = (itemToRename as IFile).PureName;
                    }
                    destName = destName.ReplaceRules(RenameRules);
                    var log = new RenameLog()
                    {
                        Before = itemToRename.Path,
                        DateTime = DateTime.Now,
                    };
                    if (itemToRename.Type == StorageItemType.File && !IncludeExtension)
                    {
                        (itemToRename as IFile).RenameIgnoreExtension(destName);
                    }
                    else
                    {
                        itemToRename.Rename(destName);
                    }
                    log.After = itemToRename.Path;
                    InnerLogList.Add(log);
                }
                WhileIsRunning = false;
            });
        }

        public async void Start()
        {
            ItemsToRename.Clear();
            InnerLogList.Clear();
            Analized = false;
            await Analize(RootFolder);
            Analized = true;
            await Rename();
        }
        public void Pause()
        {
            Paused = true;
            while (WhileIsRunning) Thread.Sleep(1);
        }
        public async void Remuse()
        {
            Paused = false;
            if (Analized)
                await Rename();
        }
        public void Stop()
        {
            Paused = true;
            while (WhileIsRunning) Thread.Sleep(1);
            ItemsToRename.Clear();
        }

        public static void Rename(IFile file, IEnumerable<RenameRule> rules, bool includeExtension = false)
        {
            if (includeExtension)
            {
                file.Rename(file.Name.ReplaceRules(rules));
            }
            else
            {
                file.RenameIgnoreExtension(file.PureName.ReplaceRules(rules));
            }
        }
    }
}
