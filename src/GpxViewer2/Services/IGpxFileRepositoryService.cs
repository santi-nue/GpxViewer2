using System.Collections.Generic;
using GpxViewer2.Model;
using GpxViewer2.Services.GpxFileStore;

namespace GpxViewer2.Services;

public interface IGpxFileRepositoryService
{
    IReadOnlyList<GpxFileRepositoryNode> GetAllLoadedNodes();

    IEnumerable<LoadedGpxFile> QueryAllLoadedFiles();

    GpxFileRepositoryNode LoadGpxFile(string filePath);

    GpxFileRepositoryNode LoadGpxFilesFromDirectory(string directoryPath);
}