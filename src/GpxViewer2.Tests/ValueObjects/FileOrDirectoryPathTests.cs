using GpxViewer2.ValueObjects;
using FluentAssertions;

namespace GpxViewer2.Tests.ValueObjects;

public class FileOrDirectoryPathTests
{
    [Fact]
    public void Equals_WhenCalledWithSamePath_ReturnsTrue()
    {
        var filePath1 = new FileOrDirectoryPath("/path/to/file1.txt");
        var filePath2 = new FileOrDirectoryPath("/path/to/file1.txt");

        filePath1.Equals(filePath2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WhenCalledWithDifferentPaths_ReturnsFalse()
    {
        var filePath1 = new FileOrDirectoryPath("/path/to/file1.txt");
        var filePath2 = new FileOrDirectoryPath("/path/to/file2.txt");

        filePath1.Equals(filePath2).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ReturnsSameHashCodeForSamePaths()
    {
        var filePath1 = new FileOrDirectoryPath("/path/to/file1.txt");
        var filePath2 = new FileOrDirectoryPath("/path/to/file1.txt");

        filePath1.GetHashCode().Should().Be(filePath2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ReturnsDifferentHashCodesForDifferentPaths()
    {
        var filePath1 = new FileOrDirectoryPath("/path/to/file1.txt");
        var filePath2 = new FileOrDirectoryPath("/path/to/file2.txt");

        filePath1.GetHashCode().Should().NotBe(filePath2.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsCorrectlyFormattedString()
    {
        var filePath = new FileOrDirectoryPath("/path/to/file.txt");

        filePath.ToString().Should().Be("/path/to/file.txt");
    }

    [Fact]
    public void ToString_ReturnsAbsolutePath()
    {
        var filePath = new FileOrDirectoryPath("file.txt");

        filePath.ToString().Should().NotBe("file.txt");
        filePath.ToString().Should().EndWith("file.txt");
    }
    
    [Fact]
    public void OperatorEqual_ReturnsTrueForSamePaths()
    {
        var filePath1 = new FileOrDirectoryPath("/path/to/file1.txt");
        var filePath2 = new FileOrDirectoryPath("/path/to/file1.txt");

        (filePath1 == filePath2).Should().BeTrue();
    }

    [Fact]
    public void OperatorEqual_ReturnsFalseForDifferentPaths()
    {
        var filePath1 = new FileOrDirectoryPath("/path/to/file1.txt");
        var filePath2 = new FileOrDirectoryPath("/path/to/file2.txt");

        (filePath1 == filePath2).Should().BeFalse();
    }

    [Fact]
    public void OperatorNotEqual_ReturnsFalseForSamePaths()
    {
        var filePath1 = new FileOrDirectoryPath("/path/to/file1.txt");
        var filePath2 = new FileOrDirectoryPath("/path/to/file1.txt");

        (filePath1 != filePath2).Should().BeFalse();
    }

    [Fact]
    public void OperatorNotEqual_ReturnsTrueForDifferentPaths()
    {
        var filePath1 = new FileOrDirectoryPath("/path/to/file1.txt");
        var filePath2 = new FileOrDirectoryPath("/path/to/file2.txt");

        (filePath1 != filePath2).Should().BeTrue();
    }
}