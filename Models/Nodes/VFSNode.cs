// Copyright (c) 2022-2023, Atypical Consulting SRL
// All rights reserved... but seriously, we're open to sharing if you ask nicely!
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;
using Atypical.VirtualFileSystem.Core.Contracts;

namespace Atypical.VirtualFileSystem.Core
{
    /// <summary>
    ///     Represents a node in a virtual file system.
    ///     A node can be a file or a directory.
    /// </summary>
    public abstract class VFSNode : IVirtualFileSystemNode
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VFSNode" /> class.
        ///     This constructor is used by derived classes.
        /// </summary>
        /// <param name="path">The path of the node.</param>
        protected VFSNode(VFSPath path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            Path = path;
        }

        /// <inheritdoc cref="IVirtualFileSystemNode.CreationTime" />
        public VFSPath Path { get; protected set; }
        
        /// <inheritdoc cref="IVirtualFileSystemNode.IsDirectory" />
        public abstract bool IsDirectory { get; }

        /// <inheritdoc cref="IVirtualFileSystemNode.IsFile" />
        public abstract bool IsFile { get; }

        /// <inheritdoc cref="IVirtualFileSystemNode.UpdatePath" />
        public virtual void UpdatePath(VFSPath path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            Path = path;
        }

        public string guid;
    }
}