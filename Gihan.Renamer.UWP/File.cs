using System;
using Gihan.Renamer.Core;
using Windows.Storage;
using NameCollisionOption = Gihan.Renamer.Core.Enums.NameCollisionOption;

namespace Gihan.Renamer.UWP
{
    public class File : Base.StorageItem, IFile
    {
        StorageFile BaseFile { get; }

        public string PureName => throw new NotImplementedException();

        public string Extension => throw new NotImplementedException();

        public IFile Copy(IFolder destinationFolder, 
            Core.Enums.NameCollisionOption option = Core.Enums.NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public IFile Copy(IFolder destinationFolder, string desiredNewName,
            Core.Enums.NameCollisionOption option = Core.Enums.NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public void Move(IFolder destinationFolder,
            Core.Enums.NameCollisionOption option = Core.Enums.NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public void Move(IFolder destinationFolder, string desiredNewName,
            Core.Enums.NameCollisionOption option = Core.Enums.NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public void RenameIgnoreExtension(string desiredName, NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public void Replace(IFile fileToReplace)
        {
            throw new NotImplementedException();
        }
    }
}
