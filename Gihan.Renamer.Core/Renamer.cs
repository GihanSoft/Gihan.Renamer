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

        public IEnumerable<bool> Rename(IEnumerable<RenameOrder> renameOrders)
        {
            var renames = new List<RenameOrder>();
            foreach (var renameOrder in renameOrders)
            {
                var renamed = true;
                try
                {
                    var item = StorageHelper.GetItem(renameOrder.Path);
                    item.Move(renameOrder.DestPath);
                    renames.Add(renameOrder);
                }
                catch (Exception err)
                {
                    renameOrder.Message = $"{err.GetType().FullName}: {err.Message}";
                    renamed = false;
                }
                yield return renamed;
            }
            var db = new AppDbContext();
            var gp = new RenameGroup()
            {
                DateTime = DateTime.Now,
                Renames = renames
            };
            db.RenameGroups.Insert(gp);
            db.Dispose();
        }
    }
}
