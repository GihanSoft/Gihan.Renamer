using System;
using System.IO;
using Gihan.Renamer.Core;
using Gihan.Renamer.Core.Enums;

using SysPath = System.IO.Path;

namespace Gihan.Renamer.SystemIO
{
    public class File : Base.StorageItem, IFile
    {
        private IFolder _parent;

        /// <summary>
        /// The parent folder of the current storage item.
        /// </summary>
        public override IFolder Parent => _parent ?? (_parent = new Folder(BaseFile.Directory));

        protected FileInfo BaseFile => (FileInfo)BaseStorageItem;

        /// <summary>
        /// The <see cref="StorageItemType"/> of this item.
        /// </summary>
        public override StorageItemType Type => StorageItemType.File;

        /// <summary>
        /// The name of the item with out the file name extension if there is one.
        /// </summary>
        public string PureName => SysPath.GetFileNameWithoutExtension(Name);

        /// <summary>
        /// The extension of current item. (intclude '.')
        /// </summary>
        public string Extension => SysPath.GetExtension(Name);

        public File(FileInfo item) : base(item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if(!item.Exists)
                throw new ArgumentException("file is not exist", nameof(item));
            if(Directory.Exists(item.FullName))
                throw new ArgumentException("this is a folder", nameof(item));
        }

        public File(string filePath) : this(new FileInfo(filePath))
        {
        }

        /// <summary>
        /// Renames the current item. This method also specifies what to do if an existing
        ///     item in the current item's location has the same name.
        /// </summary>
        /// <param name="desiredName">The desired, new name of the current item.</param>
        /// <param name="option">
        /// The enum value that determines how responds if the <see cref="desiredName"/> is the
        ///     same as the name of an existing item in the current item's location.
        ///     Default value is "<see cref="NameCollisionOption.FailIfExists"/>".
        /// </param>
        public override void Rename(string desiredName, 
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Renames the current item but ignore extension.
        ///     This method also specifies what to do if an existing
        ///     item in the current item's location has the same name.
        /// </summary>
        /// <param name="desiredName">
        /// The desired, new name of the current item.
        /// </param>
        /// <param name="option">
        /// The enum value that determines how responds if the <see cref="desiredName"/> is the
        ///     same as the name of an existing item in the current item's location.
        ///     Default value is "<see cref="NameCollisionOption.FailIfExists"/>".
        /// </param>
        public void RenameIgnoreExtension(string desiredName, 
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a copy of the file in the specified folder.
        /// This method also specifies what to do if 
        ///     a file with the same name already exists in the destination folder.
        /// </summary>
        /// <param name="destinationFolder">
        /// The destination folder where the copy of the file is created.
        /// </param>
        /// <param name="option">
        /// One of the enumeration values that determines how to handle the collision if
        ///     a file with the same name already exists in the destination folder.
        ///     Default value is <see cref="NameCollisionOption.FailIfExists"/>
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file created in the "<see cref="destinationFolder"/>".
        /// </returns>
        public IFile Copy(IFolder destinationFolder, NameCollisionOption option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a copy of the file in the specified folder and renames the copy. This
        ///     method also specifies what to do if a file with the same name already exists
        ///     in the destination folder.
        /// </summary>
        /// <param name="destinationFolder">
        /// The destination folder where the copy of the file is created.
        /// </param>
        /// <param name="desiredNewName">
        /// The new name for the copy of the file created in the "<see cref="destinationFolder"/>".
        /// </param>
        /// <param name="option">
        /// One of the enumeration values that determines how to handle the collision if a file 
        ///     with the specified "<see cref="desiredNewName"/>" already exists in the destination folder.
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file created in the "<see cref="destinationFolder"/>".
        /// </returns>
        public IFile Copy(IFolder destinationFolder, string desiredNewName, NameCollisionOption option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves the current file to the specified folder. This method also specifies 
        ///     what to do if a file with the same name already exists in the specified folder.
        /// </summary>
        /// <param name="destinationFolder">
        /// The destination folder where the file is moved.
        /// </param>
        /// <param name="option">
        /// An enum value that determines how responds if the name of current file is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        public void Move(IFolder destinationFolder, NameCollisionOption option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according
        ///     to the desired name. This method also specifies what to do if a file with the
        ///     same name already exists in the specified folder.
        /// </summary>
        /// <param name="destinationFolder">
        /// The destination folder where the file is moved.
        /// </param>
        /// <param name="desiredNewName">
        /// The desired name of the file after it is moved.
        /// </param>
        /// <param name="option">
        /// An enum value that determines how responds if the "<see cref="desiredNewName"/>" is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        public void Move(IFolder destinationFolder, string desiredNewName, NameCollisionOption option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Replaces the specified file with a copy of the current file.
        /// </summary>
        /// <param name="fileToReplace">
        /// The file to replace.
        /// </param>
        public void Replace(IFile fileToReplace)
        {
            throw new NotImplementedException();
        }
    }
}
