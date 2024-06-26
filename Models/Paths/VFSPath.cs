// Copyright (c) 2022-2023, Atypical Consulting SRL
// All rights reserved... but seriously, we're open to sharing if you ask nicely!
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Atypical.VirtualFileSystem.Core
{
    using static VFS;

    /// <summary>
    ///     Represents a file system entry (file or directory) in the virtual file system.
    /// </summary>
    public abstract class VFSPath : IComparable, IEquatable<VFSPath>
    {
        /// <summary>
        ///     Creates a new instance of <see cref="VFSPath" />.
        /// </summary>
        /// <param name="path">The path to the file system entry.</param>
        /// <exception cref="ArgumentNullException">Thrown when the path is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the path is invalid.</exception>
        protected VFSPath(string path)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (string.IsNullOrWhiteSpace(path))
                ThrowArgumentHasInvalidFormat(string.Empty);

            var vfsPath = CleanVFSPathInput(path);

            if (vfsPath == ROOT_PATH)
            {
                Value = vfsPath;
                m_Parent = null;
                return;
            }

            if (vfsPath.Contains($".{DIRECTORY_SEPARATOR}"))
            {
                ThrowArgumentHasRelativePathSegment(vfsPath);
            }

            if (vfsPath.IndexOf(DIRECTORY_SEPARATOR2, StringComparison.Ordinal) != vfsPath.LastIndexOf(DIRECTORY_SEPARATOR2, StringComparison.Ordinal))
            {
                ThrowArgumentHasInvalidFormat(vfsPath);
            }

            Value = vfsPath;
            var idx = vfsPath.IndexOf(ROOT_PATH, StringComparison.OrdinalIgnoreCase);
            WithoutRoot = vfsPath[(idx + ROOT_PATH.Length)..];
        }

        /// <summary>
        ///     Gets the path of the file system entry with the VFS prefix.
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     Gets the path of the file system entry with the VFS prefix.
        /// </summary>
        public string Str => Value;

        /// <summary>
        ///     Gets the path of the file system entry without the VFS prefix.
        /// </summary>
        public string WithoutRoot { get; }

        /// <summary>
        ///     Gets the path of the parent directory.
        /// </summary>
        public VFSDirectoryPath Parent
        {
            get
            {
                if (m_Parent == null && !IsRoot)
                {
                    var lastIndexOfParentPath = Value.LastIndexOf(DIRECTORY_SEPARATOR, StringComparison.Ordinal);
                    var parentPath = Value[..lastIndexOfParentPath] + DIRECTORY_SEPARATOR;
                    if (string.Equals(parentPath, ROOT_PATH, StringComparison.OrdinalIgnoreCase))
                    {
                        m_Parent = VFSRootPath.Root;
                    }
                    else
                    {
                        m_Parent = new VFSDirectoryPath(parentPath);
                    }
                }
                return m_Parent;
            }
        }

        private VFSDirectoryPath m_Parent;

        /// <summary>
        ///     Indicates whether the path has a parent directory.
        /// </summary>
        public bool HasParent => Parent != null;

        /// <summary>
        ///     Gets a value indicating whether the directory is the root directory.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the directory is the root directory; otherwise, <see langword="false" />.
        /// </returns>
        public bool IsRoot => string.Equals(Value, ROOT_PATH, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        ///     Gets the depth of the file system entry.
        ///     The root directory has a depth of 0.
        ///     The depth of a file is the depth of its parent directory plus one.
        ///     The depth of a directory is the depth of its parent directory plus one.
        /// </summary>
        public int Depth
        {
            get
            {
                int GetDepth()
                {
                    if (IsRoot) return -1;
                    var depth = 0;
                    var path = this;
                    while (path!.Value != ROOT_PATH)
                    {
                        path = path.Parent;
                        depth++;
                    }
                    return depth;
                }
                m_Depth ??= GetDepth();
                return m_Depth.Value;
            }
        }

        private int? m_Depth;

        /// <summary>
        ///     Gets the name of the file system entry.
        ///     The name of the root directory is <see cref="ROOT_PATH" />.
        ///     The name of a file is the name of the file with its extension.
        /// </summary>
        public string Name
        {
            get
            {
                string GetName()
                {
                    if (IsRoot) return ROOT_PATH;
                    var lastIndexOfName = Value.LastIndexOf(DIRECTORY_SEPARATOR, StringComparison.Ordinal);
                    return Value[(lastIndexOfName + 1)..];
                }
                return m_Name ??= GetName();
            }
        }

        private string m_Name;

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A value that indicates whether the current object is equal to the <paramref name="other" /> parameter.</returns>
        public virtual bool Equals(VFSPath other)
        {
            if (ReferenceEquals(null, other)) return false;
            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Cleans the input path.
        /// </summary>
        /// <param name="path">The path to clean.</param>
        /// <returns>The cleaned path.</returns>
        private static string CleanVFSPathInput(string path)
        {
            // clean up the path
            var cleanPath = path.Trim();

            // if is root path, return it
            if (cleanPath is ROOT_PATH or "")
                return cleanPath;

            // replace backslashes with forward slashes
            cleanPath = cleanPath.Replace('\\', '/');

            // clean up the path - remove leading and trailing slashes
            cleanPath = cleanPath.TrimStart('/');
            cleanPath = cleanPath.TrimEnd('/');

            // if path does not start with the root path, add it
            if (!cleanPath.StartsWith(ROOT_PATH, StringComparison.OrdinalIgnoreCase))
            {
                cleanPath = $"{ROOT_PATH}{cleanPath}";
            }

            return cleanPath;
        }

        /// <summary>
        ///     Gets the absolute path of the parent directory with depth <paramref name="depthFromRoot" />.
        ///     The root directory has a depth of 0.
        ///     The depth of a file is the depth of its parent directory plus one.
        ///     The depth of a directory is the depth of its parent directory plus one.
        /// </summary>
        /// <param name="depthFromRoot">The depth of the parent directory from the root directory.</param>
        /// <returns>The absolute path of the parent directory with depth <paramref name="depthFromRoot" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the depth is negative.</exception>
        public VFSDirectoryPath GetAbsoluteParentPath(int depthFromRoot)
        {
            if (depthFromRoot < 0)
                ThrowDepthFromRootMustBeGreaterThanOrEqualToZero(depthFromRoot);

            if (IsRoot)
                return this as VFSDirectoryPath
                       ?? throw new VirtualFileSystemException("The root path is not a directory path.");

            var path = this;
            while (path!.Depth > depthFromRoot)
                path = path.Parent;

            return path as VFSDirectoryPath
                   ?? throw new VirtualFileSystemException("The parent path is not a directory path.");
        }

        /// <summary>
        ///     Indicates whether the specified regular expression finds a match in the path.
        /// </summary>
        /// <param name="regex">The regular expression to match.</param>
        /// <returns>
        ///     <see langword="true" /> if the regular expression finds a match; otherwise, <see langword="false" />.
        /// </returns>
        public bool IsMatch(Regex regex) => regex.IsMatch(Value);

        /// <summary>
        ///     Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => Value.GetHashCode();

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is null)
                return 1;

            if (obj is not VFSPath other)
                throw new ArgumentException($"Object must be of type {nameof(VFSPath)}");

            return string.Compare(Value, other.Value, StringComparison.Ordinal);
        }

        [DoesNotReturn]
        private static void ThrowArgumentHasRelativePathSegment(string vfsPath)
        {
            var message = $"The path '{vfsPath}' contains a relative path segment.";
            throw new VirtualFileSystemException(message, new ArgumentException(message));
        }

        [DoesNotReturn]
        private static void ThrowArgumentHasInvalidFormat(string vfsPath)
        {
            var message = vfsPath.Length > 0
                ? $"The path '{vfsPath}' is invalid."
                : "An empty path is invalid.";

            throw new VirtualFileSystemException(message, new ArgumentException(message));
        }

        [DoesNotReturn]
        private static void ThrowDepthFromRootMustBeGreaterThanOrEqualToZero(int depthFromRoot)
        {
            var message = $"The depth from root must be greater than or equal to 0.\nActual value: {depthFromRoot}.";

            throw new VirtualFileSystemException(message);
        }

        /// <summary>
        ///     Determines whether the path relative to the specified path.
        /// </summary>
        /// <param name="other">The beginning of the path.</param>
        /// <returns>
        ///     <see langword="true" /> if the path relative to the specified path; otherwise, <see langword="false" />.
        /// </returns>
        public bool StartsWith(VFSPath other)
        {
            if (Value.StartsWith(other.Value, StringComparison.OrdinalIgnoreCase))
            {
                if (Value.Length == other.Value.Length)
                {
                    return true;
                }
                if (Value.Length > other.Value.Length)
                {
                    return Value[other.Value.Length] == '/';
                }
            }
            return false;
        }

        public static bool operator ==(VFSPath x, VFSPath y)
        {
            if (x is null) return y is null;
            if (y is null) return false;
            return string.Equals(x.Value, y.Value, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator !=(VFSPath x, VFSPath y)
        {
            return !(x == y);
        }
    }
}