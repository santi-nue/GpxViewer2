using System.Linq;
using System.Threading.Tasks;
using GpxViewer2.Messages;
using GpxViewer2.Services;
using GpxViewer2.Services.RecentlyOpened;
using GpxViewer2.ValueObjects;
using RolandK.InProcessMessaging;

namespace GpxViewer2.UseCases;

public class LoadGpxDirectoryUseCase(
    IGpxFileRepositoryService srvGpxFileRepository,
    IInProcessMessagePublisher srvMessagePublisher,
    IRecentlyOpenedService srvRecentlyOpened)
{
    public async Task LoadGpxDirectoryAsync(string directoryPath)
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

        srvMessagePublisher.Publish(new GpxToursSelectedMessage(loadedGpxTours));
        srvMessagePublisher.Publish(new ZoomToGpxToursRequestMessage(loadedGpxTours));

        await srvRecentlyOpened.AddOpenedAsync(
            directoryPath,
            RecentlyOpenedType.Directory);
    }
}