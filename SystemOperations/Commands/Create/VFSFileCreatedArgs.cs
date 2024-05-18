// Copyright (c) 2022-2023, Atypical Consulting SRL
// All rights reserved... but seriously, we're open to sharing if you ask nicely!
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;

namespace Atypical.VirtualFileSystem.Core
{
    /// <summary>
    /// Provides data for the FileCreated event.
    /// </summary>
    public sealed class VFSFileCreatedArgs : VFSEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VFSFileCreatedArgs"/> class.
        /// </summary>
        /// <param name="path">The path of the created file.</param>
        public VFSFileCreatedArgs(VFSFilePath path)
        {
            Path = path;
        }

        /// <summary>
        /// Gets the path of the created file.
        /// </summary>
        public VFSFilePath Path { get; }

        /// <inheritdoc />
        public override string MessageTemplate => "File with path '{0}' was created.";

        /// <inheritdoc />
        public override string Message => string.Format(MessageTemplate, Path);

        /// <inheritdoc />
        public override string MessageWithMarkup => ToMarkup("green", Path);

        /// <inheritdoc />
        public override string ToString() => Message;
    }
}