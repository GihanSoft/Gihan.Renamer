using System;

namespace Gihan.Renamer.Models.Enums
{
    [Flags]
    public enum RunStatus : byte
    {
        Start = /******/ 0b0000_0000,
        Preparing = /**/ 0b0000_0001,
        Prepared = /***/ 0b0000_0010,
        Pausing = /****/ 0b0000_0100,
        Paused = /*****/ 0b0000_1000,
        Running = /****/ 0b0001_0000,
        Ended = /******/ 0b1110_0000
    }
}