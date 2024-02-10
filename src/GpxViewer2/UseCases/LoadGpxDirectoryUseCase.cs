using System.Linq;
using GpxViewer2.Messages;
using GpxViewer2.Services;
using RolandK.InProcessMessaging;

namespace GpxViewer2.UseCases;

public class LoadGpxDirectoryUseCase(
    IGpxFileRepositoryService srvGpxFileRepository,
    IInProcessMessagePublisher srvMessagePublisher)
{
    public void LoadGpxDirectory(string directoryPath)
    {
        var loadedNode = srvGpxFileRepository.LoadGpxFilesFromDirectory(directoryPath);
        var loadedGpxFiles = loadedNode
            .GetAssociatedGpxFilesDeep()
            .ToArray();
        
        srvMessagePublisher.Publish(new GpxFileRepositoryNodesLoadedMessage([loadedNode]));
        srvMessagePublisher.Publish(new GpxFilesSelectedMessage(loadedGpxFiles));
    }
}