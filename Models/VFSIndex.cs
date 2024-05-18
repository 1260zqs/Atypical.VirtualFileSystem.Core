// Copyright (c) 2022-2023, Atypical Consulting SRL
// All rights reserved... but seriously, we're open to sharing if you ask nicely!
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Atypical.VirtualFileSystem.Core.Contracts;

namespace Atypical.VirtualFileSystem.Core
{
    /// <summary>
    /// Represents the index of the virtual file system.
    /// </summary>
    /// <remarks>
    /// The vfs index is a dictionary of vfs paths and vfs nodes.
    /// The vfs index is used to store the nodes of the virtual file system.
    /// The vfs index is sorted by the vfs paths.
    /// The vfs index is case insensitive.
    /// This class cannot be inherited.
    /// </remarks>
    public sealed class VFSIndex
    {
        private readonly SortedDictionary<VFSPath, IVirtualFileSystemNode> _index = new(new VFSPathComparer());

        /// <summary>
        /// Gets the keys of the raw index.
        /// </summary>
        public IEnumerable<VFSPath> Keys => _index.Keys;

        /// <summary>
        /// Gets the values of the raw index.
        /// </summary>
        public SortedDictionary<VFSPath, IVirtualFileSystemNode>.ValueCollection Values => _index.Values;

        /// <summary>
        /// Gets the total count of nodes in the index.
        /// </summary>
        public int Count => _index.Count;

        /// <summary>
        /// Gets a value indicating whether the index is empty.
        /// </summary>
        public bool IsEmpty => Count == 0;

        /// <summary>
        /// Gets the directories in the index.
        /// </summary>
        public IEnumerable<IDirectoryNode> Directories => Values.OfType<IDirectoryNode>();

        /// <summary>
        /// Gets the count of directories in the index.
        /// </summary>
        public int DirectoriesCount => Directories.Count();

        /// <summary>
        /// Gets the files in the index.
        /// </summary>
        public IEnumerable<IFileNode> Files => Values.OfType<IFileNode>();

        /// <summary>
        /// Gets the count of files in the index.
        /// </summary>
        public int FilesCount => Files.Count();
        
        /// <summary>
        /// Gets or sets the node at the specified entry path.
        /// </summary>
        public IVirtualFileSystemNode this[VFSPath directoryPath]
        {
            get => _index[directoryPath];
            set => _index[directoryPath] = value;
        }

        /// <summary>
        /// Removes the node with the specified key.
        /// </summary>
        public void Remove(VFSPath key) => _index.Remove(key);

        /// <summary>
        /// Tries to get the directory node at the specified directory path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="directoryNode">The directory node.</param>
        /// <returns><c>true</c> if the directory node exists; otherwise, <c>false</c>.</returns>
        public bool TryGetDirectory(VFSDirectoryPath directoryPath, [MaybeNullWhen(false)] out IDirectoryNode directoryNode)
        {
            if (_index.TryGetValue(directoryPath, out var node))
            {
                directoryNode = node as IDirectoryNode;
                return directoryNode != null;
            }

            directoryNode = null;
            return false;
        }

        /// <summary>
        /// Tries to get the file node at the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileNode">The file node.</param>
        /// <returns><c>true</c> if the file node exists; otherwise, <c>false</c>.</returns>
        public bool TryGetFile(VFSFilePath filePath, [MaybeNullWhen(false)] out IFileNode fileNode)
        {
            if (_index.TryGetValue(filePath, out var node))
            {
                fileNode = node as IFileNode;
                return fileNode != null;
            }

            fileNode = null;
            return false;
        }
        
        /// <summary>
        /// Tries to get the file or directory at the specified file path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="node">The node.</param>
        /// <returns><c>true</c> if the node exists; otherwise, <c>false</c>.</returns>
        public bool TryGet(VFSPath path, [MaybeNullWhen(false)] out IVirtualFileSystemNode node)
        {
            return _index.TryGetValue(path, out node);
        }

        /// <summary>
        /// Determines whether the index contains the specified key.
        /// </summary>
        public bool ContainsKey(VFSPath key) => _index.ContainsKey(key);

        /// <summary>
        /// Tries to add the specified node to the index.
        /// </summary>
        public bool TryAdd(VFSPath pathValue, IVirtualFileSystemNode node) => _index.TryAdd(pathValue, node);

        /// <summary>
        /// Gets the file node at the specified file path.
        /// </summary>
        public IFileNode GetFile(VFSFilePath filePath) => (IFileNode)this[filePath];

        /// <summary>
        /// Gets the directory node at the specified directory path.
        /// </summary>
        public IDirectoryNode GetDirectory(VFSDirectoryPath directoryPath) => (IDirectoryNode)this[directoryPath];

        /// <summary>
        /// Gets the paths starting with the specified directory path.
        /// </summary>
        public VFSPath[] GetPathsStartingWith(VFSDirectoryPath path)
        {
            return Keys.Where(p => p.StartsWith(path))
                .OrderByDescending(p => p.Value.Length).ToArray();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
            => $"VFS: {FilesCount} files, {DirectoriesCount} directories";
    }
}