// Copyright (c) 2022-2023, Atypical Consulting SRL
// All rights reserved... but seriously, we're open to sharing if you ask nicely!
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

namespace Atypical.VirtualFileSystem.Core
{
    /// <summary>
    /// Provides data for the DirectoryRenamed event.
    /// </summary>
    public sealed class VFSRenamedArgs : VFSEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VFSRenamedArgs"/> class.
        /// </summary>
        /// <param name="path">The path of the renamed directory.</param>
        /// <param name="oldName">The old name of the renamed directory.</param>
        /// <param name="newName">The new name of the renamed directory.</param>
        public VFSRenamedArgs(VFSPath oldPath, VFSPath newPath)
        {
            NewPath = newPath;
            OldPath = oldPath;
        }

        /// <summary>
        /// Gets the new path of the renamed directory.
        /// </summary>
        public VFSPath NewPath { get; }

        /// <summary>
        /// Gets the old path of the renamed directory.
        /// </summary>
        public VFSPath OldPath { get; }

        /// <inheritdoc />
        public override string MessageTemplate => "Directory was renamed from path '{0}' to path '{1}'.";

        /// <inheritdoc />
        public override string Message => string.Format(MessageTemplate, OldPath, NewPath);

        /// <inheritdoc />
        public override string MessageWithMarkup => ToMarkup("blue", OldPath, NewPath);

        /// <inheritdoc />
        public override string ToString() => Message;
    }
}