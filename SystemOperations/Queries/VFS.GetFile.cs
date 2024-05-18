using Atypical.VirtualFileSystem.Core.Contracts;

namespace Atypical.VirtualFileSystem.Core
{
    public partial class VFS
    {
        /// <inheritdoc cref="IVirtualFileSystem.GetFile(VFSFilePath)" />
        public IFileNode GetFile(VFSFilePath filePath) => Index.GetFile(filePath);
    }
}