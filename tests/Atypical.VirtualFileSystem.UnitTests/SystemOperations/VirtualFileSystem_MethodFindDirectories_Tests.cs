using System.Text.RegularExpressions;

namespace VirtualFileSystem.UnitTests.SystemOperations;

public class VirtualFileSystem_MethodFindDirectories_Tests : VirtualFileSystemTestsBase
{
    [Fact]
    public void FindDirectories_returns_all_directories()
    {
        // Arrange
        var vfs = CreateVFS();
        vfs.CreateDirectory(new VFSDirectoryPath("dir1"));
        vfs.CreateDirectory(new VFSDirectoryPath("dir2"));
        vfs.CreateDirectory(new VFSDirectoryPath("dir3"));

        // Act
        var directories = vfs.Directories.ToList();

        // Assert
        directories.Should().NotBeEmpty();
        directories.Should().HaveCount(3); // dir1 + dir2 + dir3
        directories.Should().Contain(d => d.Path.Value == "vfs://dir1");
        directories.Should().Contain(d => d.Path.Value == "vfs://dir2");
        directories.Should().Contain(d => d.Path.Value == "vfs://dir3");
    }

    [Fact]
    public void FindDirectories_returns_all_directories_matching_a_pattern()
    {
        // Arrange
        var vfs = CreateVFS();
        vfs.CreateDirectory(new VFSDirectoryPath("dir1"));
        vfs.CreateDirectory(new VFSDirectoryPath("dir2"));
        vfs.CreateDirectory(new VFSDirectoryPath("dir3"));

        // Act
        var regexPattern = new Regex("dir1");
        var directories = vfs.FindDirectories(regexPattern).ToList();

        // Assert
        directories.Should().NotBeEmpty();
        directories.Should().HaveCount(1);
        directories.Should().Contain(d => d.Path.Value == "vfs://dir1");
    }
}