#### [Atypical.VirtualFileSystem.Core](VirtualFileSystem.md 'VirtualFileSystem')
### [Atypical.VirtualFileSystem.Core.Contracts](VirtualFileSystem.md#Atypical.VirtualFileSystem.Core.Contracts 'Atypical.VirtualFileSystem.Core.Contracts')

## IFileNode Interface

Represents a file in a virtual file system.  
This is the base interface for all file types.

```csharp
public interface IFileNode :
Atypical.VirtualFileSystem.Core.Contracts.IVirtualFileSystemNode
```

Derived  
&#8627; [FileNode](FileNode.md 'Atypical.VirtualFileSystem.Core.FileNode')

Implements [IVirtualFileSystemNode](IVirtualFileSystemNode.md 'Atypical.VirtualFileSystem.Core.Contracts.IVirtualFileSystemNode')

| Properties | |
| :--- | :--- |
| [Content](IFileNode.Content.md 'Atypical.VirtualFileSystem.Core.Contracts.IFileNode.Content') | Gets the content of the file as a string.<br/>The encoding is in UTF-8. |
