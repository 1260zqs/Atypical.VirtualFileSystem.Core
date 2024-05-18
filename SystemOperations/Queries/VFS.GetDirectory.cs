using Atypical.VirtualFileSystem.Core.Contracts;

namespace Atypical.VirtualFileSystem.Core
{
    public partial class VFS
    {
        /// <inheritdoc cref="IVirtualFileSystem.GetDirectory(VFSDirectoryPath)" />
        public IDirectoryNode GetDirectory(VFSDirectoryPath directoryPath)
        {
            // if the path is the root path, return the root node
            if (directoryPath.IsRoot)
            {
                return Root;
            }
            return Index.GetDirectory(directoryPath);
        }

        public DirectoryNode GetFolder(VFSDirectoryPath directoryPath)
        {
            // if the path is the root path, return the root node
            if (directoryPath.IsRoot)
            {
                return (DirectoryNode)Root;
            }
            return (DirectoryNode)Index.GetDirectory(directoryPath);
        }

        public VFSNode GetEntry(VFSPath directoryPath)
        {
            if (directoryPath.IsRoot)
            {
                return Root as VFSNode;
            }
            if (Index.TryGet(directoryPath, out var node))
            {
                return node as VFSNode;
            }
            return null;
        }
    }
}