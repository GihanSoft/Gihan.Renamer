using System;
using System.Collections.Generic;
using System.Linq;
using Gihan.Renamer.Models;
using Gihan.Renamer.Models.Enums;
using Gihan.Storage.Core;
using Gihan.Storage.Core.Base;

namespace Gihan.Renamer
{
    public abstract class Renamer
    {
        protected abstract StorageHelper StorageHelper { get; }

        public IEnumerable<bool> Rename(IEnumerable<RenameOrder> renameOrders,
                                          RenameFlags renameFlags = RenameFlags.Default)
        {
            //var logs = new List<string>();
            var helper = StorageHelper.Creat();

            var db = new Db();

            db.Insert(new RenameGroupEntity
            {
                DateTime = DateTime.Now,
                RenameFlags = renameFlags
            });
            var gp = db.RenameGroups.Last();

            foreach (var renameOrder in renameOrders)
            {
                var renameLog = new RenameEntity
                {
                    FilePath = renameOrder.FilePath,
                    NewName = renameOrder.NewName,
                    Message = renameOrder.Message,
                    RenameGroupId = gp.Id
                };
                var renamed = true;
                try
                { //todo log activities
                    var item = helper.GetItem(renameOrder.FilePath);
                    if (item is IFile fileItem && !renameFlags.HasFlag(RenameFlags.Extension))
                        fileItem.RenameIgnoreExtension(renameOrder.NewName);
                    else
                        item.Rename(renameOrder.NewName);
                }
                catch (Exception err)
                {
                    renameLog.Message += $"\r\n{err.GetType().FullName}: {err.Message}"; ;
                    renamed = false;
                }
                yield return renamed;
            }
            db.Close();
        }
    }
}
