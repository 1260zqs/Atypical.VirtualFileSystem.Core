// Copyright (c) 2022-2023, Atypical Consulting SRL
// All rights reserved... but seriously, we're open to sharing if you ask nicely!
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;
using Atypical.VirtualFileSystem.Core.Contracts;

namespace Atypical.VirtualFileSystem.Core
{
    public partial class VFS
    {
        /// <inheritdoc cref="IVFSRename.Renamed" />
        public event Action<VFSRenamedArgs> Renamed;

        /// <summary>
        /// Renames a directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="newName">The new name.</param>
        /// <returns>The virtual file system.</returns>
        public IVirtualFileSystem RenameDirectory(VFSDirectoryPath directoryPath, string newName)
        {
            if (!Index.TryGetDirectory(directoryPath, out var directoryNode))
                ThrowVirtualDirectoryNotFound(directoryPath);

            // Remove the directory from its old parent directory
            if (TryGetDirectory(directoryPath.Parent, out var oldParent))
                oldParent.RemoveChild(directoryNode);

            var paths = Index.GetPathsStartingWith(directoryPath);
            var newPaths = new VFSPath[paths.Length];
            var cutStart = directoryPath.Parent.Value.Length + 1;
            var cutLength = directoryPath.Value.Length - cutStart;
            for (var i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                var newPathStr = path.Value.Remove(cutStart, cutLength);
                newPathStr = newPathStr.Insert(cutStart, newName);
                var newPath = new VFSDirectoryPath(newPathStr);
                var entry = GetEntry(path);
                entry.UpdatePath(newPath);
                Index.Remove(path);
                Index[newPath] = entry;
                newPaths[i] = newPath;
                if (path.Equals(directoryPath))
                {
                    // Add the directory to its old parent directory with the new name
                    if (TryGetDirectory(newPath.Parent, out var newParent))
                        newParent.AddChild(entry);
                }
            }
            for (var i = 0; i < paths.Length; i++)
            {
                var oldPath = paths[i];
                var newPath = newPaths[i];
                Renamed?.Invoke(new VFSRenamedArgs(oldPath, newPath));
            }
            return this;
        }

        /// <inheritdoc cref="IVFSRename.RenameFile(VFSFilePath, string)" />
        public IVirtualFileSystem RenameFile(VFSFilePath filePath, string newName)
        {
            if (!Index.TryGetFile(filePath, out var fileNode))
                ThrowVirtualFileNotFound(filePath);

            // Remove the file from its old parent directory
            if (TryGetDirectory(filePath.Parent, out var oldParent))
                oldParent.RemoveChild(fileNode);

            // update the file node with the new path
            var newFilePath = new VFSFilePath($"{filePath.Parent}/{newName}");
            fileNode.UpdatePath(newFilePath);

            // Add the file to its new parent directory
            if (TryGetDirectory(newFilePath.Parent, out var newParent))
                newParent.AddChild(fileNode);

            // update the index
            Index.Remove(filePath);
            Index[newFilePath] = fileNode;
            
            Renamed?.Invoke(new VFSRenamedArgs(filePath, newFilePath));
            return this;
        }
    }
}