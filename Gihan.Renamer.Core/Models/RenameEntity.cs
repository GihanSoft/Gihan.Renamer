using SQLite;

namespace Gihan.Renamer.Models
{
    [Table("Rename")]
    public class RenameEntity
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        [NotNull]
        public string FilePath { get; set; }
        [NotNull]
        public string NewName { get; set; }
        public string Message { get; set; }

        public long RenameGroupId { get; set; }
        //public RenameGroupEntity RenameGroup { get; set; }
    }
}
