using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Atypical.VirtualFileSystem.Core.Contracts;

namespace Atypical.VirtualFileSystem.Core
{
    public partial class VFS
    {
        /// <inheritdoc cref="IVirtualFileSystem.FindFiles(Func{IFileNode,bool})" />
        public IEnumerable<IFileNode> FindFiles(
            Func<IFileNode, bool> predicate)
            => Index.Files.Where(predicate);

        /// <inheritdoc cref="IVirtualFileSystem.FindFiles(Regex)" />
        public IEnumerable<IFileNode> FindFiles(Regex regexPattern) => FindFiles(f => f.Path.IsMatch(regexPattern));
    }
}