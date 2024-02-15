using System.Linq;
using System.Threading.Tasks;
using GpxViewer2.Messages;
using GpxViewer2.Services;
using GpxViewer2.ValueObjects;
using RolandK.InProcessMessaging;

namespace GpxViewer2.UseCases;

public class LoadGpxFileUseCase(
    IGpxFileRepositoryService srvGpxFileRepository,
    IInProcessMessagePublisher srvMessagePublisher)
{
    public void LoadGpxFile(string filePath)
    {
        var source = new FileOrDirectoryPath(filePath);

        var repositoryNode = srvGpxFileRepository.TryGetExistingNode(source);
        if (repositoryNode == null)
        {
            repositoryNode = srvGpxFileRepository.LoadFileNode(source);
            srvMessagePublisher.Publish(new GpxFileRepositoryNodesLoadedMessage([repositoryNode]));
        }
        
        var loadedGpxTours = repositoryNode
            .GetAssociatedToursDeep()
            .ToArray();
        
        srvMessagePublisher.Publish(new GpxToursSelectedMessage(loadedGpxTours));
        srvMessagePublisher.Publish(new ZoomToGpxToursRequestMessage(loadedGpxTours));
    }
}