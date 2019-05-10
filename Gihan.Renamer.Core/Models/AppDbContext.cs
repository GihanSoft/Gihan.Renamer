using System;
using LiteDB;

namespace Gihan.Renamer.Models
{
    public class AppDbContext : IDisposable
    {
        public LiteDatabase Database { get; set; }

        public LiteCollection<RenameGroup> RenameGroups => Database.GetCollection<RenameGroup>();

        public AppDbContext(string connectionString = @"data.db", BsonMapper mapper = null, Logger log = null) 
        {
            Database = new LiteDatabase(connectionString, mapper, log);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
