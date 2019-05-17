using System;
using LiteDB;

namespace Gihan.Renamer.Models
{
    public class RenameDbContext : IDisposable
    {
        public LiteDatabase Database { get; set; }

        public LiteCollection<RenameGroup> RenameGroups => Database.GetCollection<RenameGroup>();
        public LiteCollection<RenameProcess> Processes => Database.GetCollection<RenameProcess>();

        public RenameDbContext(string connectionString = @"data.db", BsonMapper mapper = null, Logger log = null) 
        {
            Database = new LiteDatabase(connectionString, mapper, log);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
