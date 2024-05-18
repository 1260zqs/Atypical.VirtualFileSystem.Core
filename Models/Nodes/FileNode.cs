// Copyright (c) 2022-2023, Atypical Consulting SRL
// All rights reserved... but seriously, we're open to sharing if you ask nicely!
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;
using System.Diagnostics.CodeAnalysis;
using Atypical.VirtualFileSystem.Core.Contracts;

namespace Atypical.VirtualFileSystem.Core
{
    /// <summary>
    ///     Represents a file in the virtual file system.
    /// </summary>
    public class FileNode : VFSNode, IFileNode
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FileNode" /> class.
        ///     Creates a new file node.
        ///     The file is created with the current date and time as creation and last modification date.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <param name="content">The content of the file.</param>
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameterInConstructor")]
        public FileNode(VFSFilePath filePath) : base(filePath)
        {
            Path = filePath;
        }

        /// <inheritdoc cref="IVirtualFileSystemNode.IsDirectory" />
        public override bool IsDirectory => false;

        /// <inheritdoc cref="IVirtualFileSystemNode.IsFile" />
        public override bool IsFile => true;

        /// <summary>
        ///     Returns a string that represents the path of the file.
        /// </summary>
        /// <returns>A string that represents the path of the file.</returns>
        public override string ToString() => Path.ToString();

        public bool Equals(FileNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.Path == other.Path;
        }

        public static bool operator ==(FileNode x, FileNode y)
        {
            if (x is null) return y is null;
            if (y is null) return false;
            return x.Path == y.Path;
        }

        public static bool operator !=(FileNode x, FileNode y)
        {
            return !(x == y);
        }
    }
}