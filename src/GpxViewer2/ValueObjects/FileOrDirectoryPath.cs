using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxViewer2.ValueObjects;

/// <summary>
/// This ValueType ensures that more paths to the same file can not differ from each other.
/// </summary>
public readonly struct FileOrDirectoryPath(string path)
{
    public static readonly FileOrDirectoryPath Empty = new();
    
    private readonly string? _path = System.IO.Path.GetFullPath(path);

    public string Path => _path ?? string.Empty;

    public bool Equals(FileOrDirectoryPath other)
    {
        return this.Path == other.Path;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is FileOrDirectoryPath other && this.Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return this.Path.GetHashCode();
    }

    public static bool operator ==(FileOrDirectoryPath left, FileOrDirectoryPath right)
    {
        return left.Path == right.Path;
    }

    public static bool operator !=(FileOrDirectoryPath left, FileOrDirectoryPath right)
    {
        return left.Path != right.Path;
    }
}