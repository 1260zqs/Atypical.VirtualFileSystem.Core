#### [Atypical.VirtualFileSystem.Core](VirtualFileSystem.md 'VirtualFileSystem')
### [Atypical.VirtualFileSystem.Core](VirtualFileSystem.md#Atypical.VirtualFileSystem.Core 'Atypical.VirtualFileSystem.Core')

## VFSRootPath Class

Represents the root directory of the virtual file system.

```csharp
public class VFSRootPath : Atypical.VirtualFileSystem.Core.VFSDirectoryPath,
System.IEquatable<Atypical.VirtualFileSystem.Core.VFSRootPath>
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [VFSPath](VFSPath.md 'Atypical.VirtualFileSystem.Core.VFSPath') &#129106; [VFSDirectoryPath](VFSDirectoryPath.md 'Atypical.VirtualFileSystem.Core.VFSDirectoryPath') &#129106; VFSRootPath

Implements [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[VFSRootPath](VFSRootPath.md 'Atypical.VirtualFileSystem.Core.VFSRootPath')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')

| Constructors | |
| :--- | :--- |
| [VFSRootPath()](VFSRootPath.VFSRootPath().md 'Atypical.VirtualFileSystem.Core.VFSRootPath.VFSRootPath()') | Represents the root directory of the virtual file system. |

| Methods | |
| :--- | :--- |
| [ToString()](VFSRootPath.ToString().md 'Atypical.VirtualFileSystem.Core.VFSRootPath.ToString()') | Returns a string that represents the current object.<br/>The string representation of the root directory is the constant [ROOT_PATH](VFS.ROOT_PATH.md 'Atypical.VirtualFileSystem.Core.VFS.ROOT_PATH'). |

| Operators | |
| :--- | :--- |
| [implicit operator string(VFSRootPath)](VFSRootPath.implicitoperatorstring(VFSRootPath).md 'Atypical.VirtualFileSystem.Core.VFSRootPath.op_Implicit string(Atypical.VirtualFileSystem.Core.VFSRootPath)') | Implicit conversion to string<br/>This allows you to use a [VFSRootPath](VFSRootPath.md 'Atypical.VirtualFileSystem.Core.VFSRootPath') as a string. |
