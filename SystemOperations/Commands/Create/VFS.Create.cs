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
        /// <inheritdoc cref="IVFSCreate.DirectoryCreated" />
        public event Action<VFSDirectoryCreatedArgs> DirectoryCreated;

        /// <inheritdoc cref="IVFSCreate.FileCreated" />
        public event Action<VFSFileCreatedArgs> FileCreated;

        /// <inheritdoc cref="IVFSCreate.CreateDirectory(VFSDirectoryPath)" />
        public DirectoryNode CreateDirectory(VFSDirectoryPath directoryPath)
        {
            if (directoryPath.IsRoot)
                ThrowCannotCreateRootDirectory();

            if (directoryPath.Parent is null)
                ThrowCannotCreateDirectoryWithoutParent();

            var directory = new DirectoryNode(directoryPath);
            AddToIndex(directory);

            TryGetDirectory(directoryPath.Parent, out var parent);
            parent?.AddChild(directory);

            DirectoryCreated?.Invoke(new VFSDirectoryCreatedArgs(directoryPath));
            return directory;
        }

        /// <inheritdoc cref="IVFSCreate.CreateFile(VFSFilePath,string?)" />
        public FileNode CreateFile(VFSFilePath filePath)
        {
            if (filePath.Parent is null)
                ThrowCannotCreateDirectoryWithoutParent();

            var file = new FileNode(filePath);
            AddToIndex(file);

            TryGetDirectory(filePath.Parent, out var parent);
            parent?.AddChild(file);

            FileCreated?.Invoke(new VFSFileCreatedArgs(filePath));
            return file;
        }

        public VFSNode CreateFileOrDirectory(string filePath, bool file)
        {
            if (file)
            {
                return CreateFile(filePath);
            }
            return CreateDirectory(filePath);
        }
    }
}