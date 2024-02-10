using System.Collections.Generic;
using GpxViewer2.Model;
using GpxViewer2.ValueObjects;

namespace GpxViewer2.Services.GpxFileStore;

public class GpxFileRepositoryService : IGpxFileRepositoryService
{
    private List<GpxFileRepositoryNode> _loadedNodes = new();
    
    /// <inheritdoc />
    public IReadOnlyList<GpxFileRepositoryNode> GetAllLoadedNodes()
    {
        return _loadedNodes;
    }

    /// <inheritdoc />
    public IEnumerable<LoadedGpxFile> QueryAllLoadedFiles()
    {
        foreach (var actLoadedNode in _loadedNodes)
        {
            foreach (var actLoadedFile in actLoadedNode.GetAssociatedGpxFilesDeep())
            {
                yield return actLoadedFile;
            }
        }
    }

    /// <inheritdoc />
    public GpxFileRepositoryNode LoadGpxFile(string filePath)
    {
        var fileOrDirectoryPath = new FileOrDirectoryPath(filePath);
        var gpxFileNode = new GpxFileRepositoryNodeFile(fileOrDirectoryPath);
        _loadedNodes.Add(gpxFileNode);
        return gpxFileNode;
    }

    /// <inheritdoc />
    public GpxFileRepositoryNode LoadGpxFilesFromDirectory(string directoryPath)
    {
        var fileOrDirectoryPath = new FileOrDirectoryPath(directoryPath);
        var gpxDirectoryNode = new GpxFileRepositoryNodeDirectory(fileOrDirectoryPath);
        _loadedNodes.Add(gpxDirectoryNode);
        return gpxDirectoryNode;
    }
}