using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Atypical.VirtualFileSystem.Core.Contracts;

namespace Atypical.VirtualFileSystem.Core
{
    public partial class VFS
    {
        /// <inheritdoc cref="IVirtualFileSystem.TryGetDirectory(VFSDirectoryPath,out IDirectoryNode?)" />
        public bool TryGetDirectory(VFSDirectoryPath directoryPath, [NotNullWhen(true)] out IDirectoryNode directory)
        {
            if (directoryPath is null)
            {
                directory = null;
                return false;
            }

            directory = GetEntry(directoryPath) as DirectoryNode;
            return directory != null;
        }
    }
}