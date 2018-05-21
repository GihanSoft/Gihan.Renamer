namespace Gihan.Renamer.Models.Enums
{
    public enum NameCollisionOption
    {
        /// <summary>
        /// Automatically append a number to the base of the specified name if the file or folder already exists.
        /// </summary>
        GenerateUniqueName,
        /// <summary>
        /// Replace the existing item if the file or folder already exists.
        /// </summary>
        ReplaceExisting,
        /// <summary>
        /// Raise an exception of type **System.Exception** if the file or folder already exists.
        /// </summary>
        FailIfExists,
    }
}