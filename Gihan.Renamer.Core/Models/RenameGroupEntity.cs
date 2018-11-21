using Gihan.Renamer.Models.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gihan.Renamer.Models
{
    [Table("RenameGroup")]
    public class RenameGroupEntity
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        [NotNull]
        public RenameFlags RenameFlags { get; set; }
    }
}
