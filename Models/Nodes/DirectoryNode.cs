// Copyright (c) 2022-2023, Atypical Consulting SRL
// All rights reserved... but seriously, we're open to sharing if you ask nicely!
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Atypical.VirtualFileSystem.Core.Contracts;

namespace Atypical.VirtualFileSystem.Core
{
    /// <summary>
    ///     Represents a directory in the virtual file system.
    /// </summary>
    public class DirectoryNode : VFSNode, IDirectoryNode
    {
        private readonly SortedDictionary<string, IDirectoryNode> _directories = new();
        private readonly SortedDictionary<string, IFileNode> _files = new();

        /// <summary>
        ///     Initializes a new instance of the <see cref="DirectoryNode" /> class.
        ///     Creates a new directory node.
        ///     The directory is created with the current date and time as creation and last modification date.
        /// </summary>
        /// <param name="directoryPath">The path of the directory.</param>
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameterInConstructor")]
        public DirectoryNode(VFSDirectoryPath directoryPath) : base(directoryPath)
        {
            Path = directoryPath;
        }

        /// <inheritdoc cref="IDirectoryNode.Directories" />
        public ICollection<IDirectoryNode> Directories => _directories.Values;

        /// <inheritdoc cref="IDirectoryNode.Files" />
        public ICollection<IFileNode> Files => _files.Values;

        /// <inheritdoc cref="IVirtualFileSystemNode.IsDirectory" />
        public override bool IsDirectory => true;

        /// <inheritdoc cref="IVirtualFileSystemNode.IsFile" />
        public override bool IsFile => false;

        public bool Empty => _files.Count == 0 && _directories.Count == 0;

        public bool HasSubFolders => _directories.Count > 0;

        public string Name => Path.Name;

        /// <inheritdoc cref="IDirectoryNode.AddChild(IVirtualFileSystemNode)" />
        public void AddChild(IVirtualFileSystemNode node)
        {
            if (node.IsDirectory)
            {
                _directories.Add(node.Path.Value, (IDirectoryNode)node);
            }
            else if (node.IsFile)
            {
                _files.Add(node.Path.Value, (IFileNode)node);
            }
            else throw new InvalidOperationException("Cannot add a node that is neither a file nor a directory.");
        }

        /// <inheritdoc cref="IDirectoryNode.RemoveChild(IVirtualFileSystemNode)" />
        public void RemoveChild(IVirtualFileSystemNode node)
        {
            if (node.IsDirectory)
            {
                _directories.Remove(node.Path.Value);
            }
            else if (node.IsFile)
            {
                _files.Remove(node.Path.Value);
            }
            else throw new InvalidOperationException("Cannot remove a node that is neither a file nor a directory.");
        }

        /// <summary>
        ///     Returns a string that represents the path of the directory.
        /// </summary>
        /// <returns>A string that represents the path of the directory.</returns>
        public override string ToString() => Path.ToString();

        public static bool operator ==(DirectoryNode x, DirectoryNode y)
        {
            if (x is null) return y is null;
            if (y is null) return false;
            return x.Path == y.Path;
        }

        public static bool operator !=(DirectoryNode x, DirectoryNode y)
        {
            return !(x == y);
        }
    }
}