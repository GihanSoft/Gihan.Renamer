using SQLite;
using System.Collections.Generic;

namespace Gihan.Renamer.Models
{
    public class Db : SQLite.SQLiteConnection
    {
        public Db(bool storeDateTimeAsTicks = true, object key = null) : 
            base("Data/log.db3", storeDateTimeAsTicks, key)
        {
            CreateTables<RenameEntity, 
                         RenameGroupEntity>();
        }

        public TableQuery<RenameEntity> Renames => Table<RenameEntity>();
        public TableQuery<RenameGroupEntity> RenameGroups => Table<RenameGroupEntity>();
    }
}
