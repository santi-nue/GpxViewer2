using System.Collections.Generic;
using GpxViewer2.Services.GpxFileStore;

namespace GpxViewer2.Services;

public interface IGpxFileRepositoryService
{
    IReadOnlyList<GpxFileRepositoryNode> GetAllLoadedNodes();

    GpxFileRepositoryNode LoadGpxFile(string filePath);

    GpxFileRepositoryNode LoadGpxFilesFromDirectory(string directoryPath);
}