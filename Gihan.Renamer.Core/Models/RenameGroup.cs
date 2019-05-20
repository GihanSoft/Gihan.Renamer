using Gihan.Renamer.Models.Enums;
using System;
using System.Collections.Generic;

namespace Gihan.Renamer.Models
{
    public class RenameGroup
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public IEnumerable<MoveOrder> Renames { get; set; }
    }
}
