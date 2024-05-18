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
        /// <inheritdoc cref="IVFSDelete.Deleted" />
        public event Action<VFSDeletedArgs> Deleted;

        /// <inheritdoc cref="IVFSDelete.DeleteDirectory(VFSDirectoryPath)" />
        public IVirtualFileSystem DeleteDirectory(VFSDirectoryPath directoryPath)
        {
            // cannot delete the root directory
            if (directoryPath.IsRoot)
                ThrowCannotDeleteRootDirectory();

            // try to get the directory
            if (!Index.TryGetDirectory(directoryPath, out _))
                ThrowVirtualDirectoryNotFound(directoryPath);

            // find the path and its children in the index
            var paths = Index.GetPathsStartingWith(directoryPath);

            // remove the paths from the index
            foreach (var path in paths)
            {
                var node = Index[path];
                if (GetEntry(path.Parent) is DirectoryNode upper)
                {
                    upper.RemoveChild(node);
                }
                Index.Remove(path);
                Deleted?.Invoke(new VFSDeletedArgs(path));
            }
            
            return this;
        }

        /// <inheritdoc cref="IVFSDelete.DeleteFile(VFSFilePath)" />
        public IVirtualFileSystem DeleteFile(VFSFilePath filePath)
        {
            if (!Index.TryGetFile(filePath, out var fileNode))
                ThrowVirtualFileNotFound(filePath);

            // Remove the file from its parent directory
            if (TryGetDirectory(filePath.Parent, out var parent))
                parent.RemoveChild(fileNode);

            // remove the file from the index
            Index.Remove(filePath);

            Deleted?.Invoke(new VFSDeletedArgs(filePath));
            return this;
        }
    }
}