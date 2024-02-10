using System.Linq;
using System.Threading.Tasks;
using GpxViewer2.Messages;
using GpxViewer2.Services;
using RolandK.InProcessMessaging;

namespace GpxViewer2.UseCases;

public class LoadGpxFileUseCase(
    IGpxFileRepositoryService srvGpxFileRepository,
    IInProcessMessagePublisher srvMessagePublisher)
{
    public void LoadGpxFile(string filePath)
    {
        var loadedNode = srvGpxFileRepository.LoadGpxFile(filePath);
        var loadedTours = loadedNode
            .GetAssociatedToursDeep()
            .ToArray();
        
        srvMessagePublisher.Publish(new GpxFileRepositoryNodesLoadedMessage([loadedNode]));
        srvMessagePublisher.Publish(new GpxToursSelectedMessage(loadedTours));
    }
}