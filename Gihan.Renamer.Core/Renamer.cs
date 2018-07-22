using Gihan.Storage.Core;
using Gihan.Storage.Core.Base;
using Gihan.Renamer.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Gihan.Storage.Core.Enums;
using System.Threading;
using Gihan.Helpers.String;

using SysPath = System.IO.Path;

// ReSharper disable once CheckNamespace
namespace Gihan.Renamer.Core
{
    public class Renamer
    {
        private enum RunStatus : byte
        {
            Stoped,
            Preparing,
            Prepared,
            Pausing,
            Paused,
            Running,
        }

        private RunStatus Status { get; set; }
        protected IFolder RootFolder { get; }
        protected RenameRule[] RenameRules { get; }
        protected RenameFlags Flags { get; }

        protected List<IStorageItem> ItemsToRename { get; }

        private List<RenameLog> InnerLogList { get; }
        public IReadOnlyList<RenameLogReadOnly> LogList =>
                InnerLogList.Cast<RenameLogReadOnly>().ToList().AsReadOnly();


        private int RenameCounter { get; set; }
        public int RenamedCount => RenameCounter;
        public int TrueRenamedCount
        {
            get
            {
                var count = 0;
                foreach (var item in InnerLogList)
                {
                    if (item.DateTime != null && item.Before != item.After) count++;
                }
                return count;
            }
        }

        public event EventHandler<IReadOnlyList<RenameLogReadOnly>> Prepared;
        public event EventHandler<RenameLogReadOnly> FileRenamed;
        public event EventHandler<IReadOnlyList<RenameLogReadOnly>> Renamed;

        public event EventHandler<Exception> ExceptionThrowed;

        public Renamer(
            IFolder folder
            , IEnumerable<RenameRule> renameRules
            //, bool includeExtension = false
            , RenameFlags flags = RenameFlags.Default
            )
        {
            Status = RunStatus.Stoped;
            RootFolder = folder;
            RenameRules = renameRules.ToArray();
            Flags = flags;
            ItemsToRename = new List<IStorageItem>();
            InnerLogList = new List<RenameLog>();
            RenameCounter = -1;

            Prepare();
        }

        /// <summary>
        /// Prepare every thing for renaming.
        /// Finding items for rename and produce new names for them
        /// </summary>
        private async void Prepare()
        {
            if (Status != RunStatus.Stoped) return;
            Status = RunStatus.Preparing;
            await Find(RootFolder);
            await Process();
            Status = RunStatus.Prepared;
            Prepared?.Invoke(this, LogList);
        }

        /// <summary>
        /// Find items for renaming and add them in <see cref="ItemsToRename"/>
        /// </summary>
        /// <param name="folder">
        /// Folder to search in it for items
        /// </param>
        /// <returns>nothing</returns>
        private async Task Find(IFolder folder)
        {
            await Task.Run(async () =>
            {
                try
                {
                    ItemsToRename.AddRange(folder.GetFiles());
                    if (Flags.HasFlag(RenameFlags.SubFolders))
                    {
                        foreach (var item in folder.GetFolders())
                        {
                            await Find(item);
                            if (Flags.HasFlag(RenameFlags.Folder))
                                ItemsToRename.Add(item);
                        }
                    }
                }
                catch (Exception err)
                {
                    ExceptionThrowed?.Invoke(this, err);
                }
            });
        } //done

        /// <summary>
        /// Produce new name for items.
        /// </summary>
        /// <returns>no thing</returns>
        private async Task Process()
        {
            await Task.Run(() =>
            {
                try
                {
                    foreach (var storageItem in ItemsToRename)
                    {
                        var log = new RenameLog();
                        try
                        {
                            log.Before = storageItem.Path;
                            string currentName;
                            if (storageItem.Type == StorageItemType.Folder ||
                                Flags.HasFlag(RenameFlags.Extension))
                            {
                                currentName = storageItem.Name;
                            }
                            else
                            {
                                currentName = (storageItem as IFile)?.PureName;
                            }

                            var patterns = RenameRules.Select(r => r.ToTuple());
                            var destName = currentName.Replaces(patterns);

                            var destPath = SysPath.Combine(storageItem.Parent.Path, destName);
                            if (!Flags.HasFlag(RenameFlags.Extension) &&
                                storageItem.Type == StorageItemType.File)
                            {
                                destPath += (storageItem as IFile)?.Extension;
                            }

                            var destExist = storageItem.CheckExist(destPath);
                            var destWillExist = InnerLogList.Any(l => l.After == destPath);
                            
                            if (destExist)
                                throw new Exception("A item with same name exist");

                            if (destWillExist)
                                throw new Exception("there is another item that name of it will be same");
                                
                            log.After = destPath;
                        }
                        catch (Exception err)
                        {
                            log.After = nameof(Exception) + " : " + err.Message;
                            ExceptionThrowed?.Invoke(this, err);
                        }
                        InnerLogList.Add(log);
                    }
                }
                catch (Exception err)
                {
                    ExceptionThrowed?.Invoke(this, err);
                }
            });
        }//done

        private async Task Rename()
        {
            await Task.Run(() =>
            {
                try
                {
                    Status = RunStatus.Running;
                    for (var log = InnerLogList[RenameCounter]; RenameCounter < ItemsToRename.Count; 
                            RenameCounter++, 
                            log.DateTime = DateTime.Now,
                            FileRenamed?.Invoke(this, (RenameLogReadOnly)InnerLogList[RenameCounter]))
                    {
                        if (Status == RunStatus.Pausing)
                        {
                            Status = RunStatus.Paused;
                            break;
                        }
                        
                        if(log.Before == log.After) continue;
                        if(log.After.StartsWith(nameof(Exception))) continue;

                        try
                        {
                            var destName = SysPath.GetFileName(log.After);
                            var item = ItemsToRename[RenameCounter];

                            item.Rename(destName);



                        }
                        catch (Exception err)
                        {
                            log.After = nameof(Exception) + " : " + err.Message;
                            ExceptionThrowed?.Invoke(this, err);
                        }
                        //todo : complete
                    }
                }
                catch (Exception err)
                {
                    ExceptionThrowed?.Invoke(this, err);
                }
            });
        }//done

        //todo : add status conditions for below fuctions

        public async void Start()
        {
            while (Status == RunStatus.Preparing)
            {
                Thread.Sleep(10);
            }

            await Rename();

            //event
            if (Status == RunStatus.Paused) return;

            Renamed?.Invoke(this, LogList);
            Status = RunStatus.Stoped;
        }

        public void Pause()
        {
            Status = RunStatus.Pausing;
            while (Status != RunStatus.Paused) Thread.Sleep(10);
        }

        public void Stop()
        {
            Pause();

            Status = RunStatus.Stoped;
        }

        public static void Rename(IFile file, IEnumerable<RenameRule> rules, bool includeExtension = false)
        {
            var currentName = includeExtension ? file.Name : file.PureName;
            var patterns = rules.Select(r => new Tuple<string, string>(r.From, r.To));
            var destName = currentName.Replaces(patterns);

            if (destName == currentName) return;
            if (includeExtension)
                file.Rename(destName);
            else
                file.RenameIgnoreExtension(destName);
        }
    }
}
