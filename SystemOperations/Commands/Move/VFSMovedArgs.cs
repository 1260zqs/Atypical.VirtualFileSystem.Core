// Copyright (c) 2022-2023, Atypical Consulting SRL
// All rights reserved... but seriously, we're open to sharing if you ask nicely!
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;

namespace Atypical.VirtualFileSystem.Core
{
    /// <summary>
    /// Provides data for the DirectoryMoved event.
    /// </summary>
    public sealed class VFSMovedArgs : VFSEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VFSMovedArgs"/> class.
        /// </summary>
        /// <param name="sourcePath">The source path of the moved directory.</param>
        /// <param name="destinationPath">The destination path of the moved directory.</param>
        public VFSMovedArgs(VFSPath sourcePath, VFSPath destinationPath)
        {
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
        }

        /// <summary>
        /// Gets the source path of the moved directory.
        /// </summary>
        public VFSPath SourcePath { get; }

        /// <summary>
        /// Gets the destination path of the moved directory.
        /// </summary>
        public VFSPath DestinationPath { get; }

        /// <inheritdoc />
        public override string MessageTemplate => "Directory was moved from path '{0}' to path '{1}'.";

        /// <inheritdoc />
        public override string Message => string.Format(MessageTemplate, SourcePath, DestinationPath);

        /// <inheritdoc />
        public override string MessageWithMarkup => ToMarkup("yellow", SourcePath, DestinationPath);

        /// <inheritdoc />
        public override string ToString() => Message;
    }
}