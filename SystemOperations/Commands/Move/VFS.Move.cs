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
        /// <inheritdoc cref="IVFSMove.Moved" />
        public event Action<VFSMovedArgs> Moved;

        /// <inheritdoc cref="IVFSMove.MoveDirectory(VFSDirectoryPath, VFSDirectoryPath)" />
        public IVirtualFileSystem MoveDirectory(VFSDirectoryPath sourceDirectoryPath, VFSDirectoryPath destinationDirectoryPath)
        {
            if (!Index.TryGetDirectory(sourceDirectoryPath, out var directoryNode))
            {
                ThrowVirtualDirectoryNotFound(sourceDirectoryPath);
            }

            // Remove the directory from its old parent directory
            if (TryGetDirectory(sourceDirectoryPath.Parent, out var oldParent))
            {
                oldParent.RemoveChild(directoryNode);
            }
            
            var paths = Index.GetPathsStartingWith(sourceDirectoryPath);
            var newPaths = new VFSPath[paths.Length];
            var cutStart = sourceDirectoryPath.Value.Length;
            for (var i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                var newPathStr = path.Value.Substring(cutStart, path.Value.Length - cutStart);
                newPathStr = destinationDirectoryPath.Value + newPathStr;
                var newPath = new VFSDirectoryPath(newPathStr);
                var entry = GetEntry(path);
                entry.UpdatePath(newPath);
                Index.Remove(path);
                Index[newPath] = entry;
                newPaths[i] = newPath;
                if (path.Equals(sourceDirectoryPath))
                {
                    if (TryGetDirectory(newPath.Parent, out var newParent))
                    {
                        newParent.AddChild(entry);
                    }
                }
            }
            
            for (var i = 0; i < paths.Length; i++)
            {
                var oldPath = paths[i];
                var newPath = newPaths[i];
                Moved?.Invoke(new VFSMovedArgs(oldPath, newPath));
            }
            return this;
        }

        /// <inheritdoc cref="IVFSMove.MoveFile(VFSFilePath, VFSFilePath)" />
        public IVirtualFileSystem MoveFile(VFSFilePath sourceFilePath, VFSFilePath destinationFilePath)
        {
            if (!Index.TryGetFile(sourceFilePath, out var fileNode))
                ThrowVirtualFileNotFound(sourceFilePath);

            // Remove the file from its old parent directory
            if (TryGetDirectory(sourceFilePath.Parent, out var oldParent))
                oldParent.RemoveChild(fileNode);

            fileNode.UpdatePath(destinationFilePath);

            // Add the file to its new parent directory
            if (TryGetDirectory(destinationFilePath.Parent, out var newParent))
                newParent.AddChild(fileNode);

            Index.Remove(sourceFilePath);
            Index[destinationFilePath] = fileNode;

            Moved?.Invoke(new VFSMovedArgs(sourceFilePath, destinationFilePath));
            return this;
        }
    }
}