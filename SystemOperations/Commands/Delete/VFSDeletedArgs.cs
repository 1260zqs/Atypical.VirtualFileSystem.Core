// Copyright (c) 2022-2023, Atypical Consulting SRL
// All rights reserved... but seriously, we're open to sharing if you ask nicely!
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;

namespace Atypical.VirtualFileSystem.Core
{
    /// <summary>
    /// Provides data for the DirectoryDeleted event.
    /// </summary>
    public sealed class VFSDeletedArgs : VFSEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VFSDeletedArgs"/> class.
        /// </summary>
        /// <param name="path">The path of the deleted directory.</param>
        public VFSDeletedArgs(VFSPath path)
        {
            Path = path;
        }

        /// <summary>
        /// Gets the path of the deleted directory.
        /// </summary>
        public VFSPath Path { get; }

        /// <inheritdoc />
        public override string MessageTemplate => "Directory with path '{0}' was deleted.";

        /// <inheritdoc />
        public override string Message => string.Format(MessageTemplate, Path);

        /// <inheritdoc />
        public override string MessageWithMarkup => ToMarkup("red", Path);

        /// <inheritdoc />
        public override string ToString() => Message;
    }
}