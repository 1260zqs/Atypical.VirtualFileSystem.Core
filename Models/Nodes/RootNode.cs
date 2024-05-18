// Copyright (c) 2022-2023, Atypical Consulting SRL
// All rights reserved... but seriously, we're open to sharing if you ask nicely!
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using Atypical.VirtualFileSystem.Core.Contracts;

namespace Atypical.VirtualFileSystem.Core
{
    /// <summary>
    ///     Represents the root directory of the virtual file system.
    /// </summary>
    public class RootNode : DirectoryNode, IRootNode
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RootNode" /> class.
        /// </summary>
        public RootNode() : base(VFSRootPath.Root)
        {
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        ///     For <see cref="RootNode" /> this is always the constant string <cref see="ROOT_PATH" />.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Path.ToString();
    }
}