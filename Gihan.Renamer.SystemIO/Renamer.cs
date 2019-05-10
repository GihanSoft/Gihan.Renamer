using Gihan.Storage.Core.Base;

namespace Gihan.Renamer.SystemIO
{
    public class Renamer : Gihan.Renamer.Renamer
    {
        protected override StorageHelper StorageHelper { get; }
        public Renamer()
        {
            Storage.SystemIO.Base.StorageHelper.Init(); //to initialize as SysIO
            StorageHelper = StorageHelper.Creat(); 
        }
    }
}
