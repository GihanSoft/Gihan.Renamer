using System;
using Gihan.Renamer.Core;
using Gihan.Renamer.Core.Enums;

namespace Gihan.Renamer.SystemIO
{
    public class File : Base.StorageItem, IFile
    {
        public string PureName => throw new NotImplementedException();

        public string Extension => throw new NotImplementedException();

        public IFile Copy(IFolder destinationFolder, NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public IFile Copy(IFolder destinationFolder, string desiredNewName, NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public void Move(IFolder destinationFolder, NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public void Move(IFolder destinationFolder, string desiredNewName, NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public void Replace(IFile fileToReplace)
        {
            throw new NotImplementedException();
        }
    }
}
