using System.Linq;
using GpxViewer2.Messages;
using GpxViewer2.Services;
using GpxViewer2.ValueObjects;
using RolandK.InProcessMessaging;

namespace GpxViewer2.UseCases;

public class LoadGpxDirectoryUseCase(
    IGpxFileRepositoryService srvGpxFileRepository,
    IInProcessMessagePublisher srvMessagePublisher)
{
    public void LoadGpxDirectory(string directoryPath)
    {
        var source = new FileOrDirectoryPath(directoryPath);
        
        var repositoryNode = srvGpxFileRepository.TryGetExistingNode(source);
        if (repositoryNode == null)
        {
            repositoryNode = srvGpxFileRepository.LoadDirectoryNode(source);
            srvMessagePublisher.Publish(new GpxFileRepositoryNodesLoadedMessage([repositoryNode]));
        }
        
        var loadedGpxTours = repositoryNode
            .GetAssociatedToursDeep()
            .ToArray();

        srvMessagePublisher.Publish(new GpxFilesSelectedMessage(loadedGpxTours));
    }
}