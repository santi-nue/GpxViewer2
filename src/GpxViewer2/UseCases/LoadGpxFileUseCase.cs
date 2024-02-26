using System.Linq;
using System.Threading.Tasks;
using GpxViewer2.Messages;
using GpxViewer2.Services;
using GpxViewer2.Services.RecentlyOpened;
using GpxViewer2.ValueObjects;
using RolandK.AvaloniaExtensions.ViewServices;
using RolandK.InProcessMessaging;

namespace GpxViewer2.UseCases;

public class LoadGpxFileUseCase(
    IGpxFileRepositoryService srvGpxFileRepository,
    IInProcessMessagePublisher srvMessagePublisher,
    IRecentlyOpenedService srvRecentlyOpened)
{
    public async Task LoadGpxFileAsync(
        IOpenFileViewService srvOpenFileDialog)
    {
        var fileToOpen = await srvOpenFileDialog.ShowOpenFileDialogAsync(
            [new FileDialogFilter("GPX-Files (*.gpx)", ["*.gpx"])],
            "Load GPX-File");
        if (string.IsNullOrEmpty(fileToOpen)) { return; }

        await this.LoadGpxFileAsync(fileToOpen);
    }

    public async Task LoadGpxFileAsync(
        string fileToOpen)
    {
        var source = new FileOrDirectoryPath(fileToOpen);

        var repositoryNode = srvGpxFileRepository.TryGetExistingNode(source);
        var newNodeLoaded = false;
        if (repositoryNode == null)
        {
            repositoryNode = srvGpxFileRepository.LoadFileNode(source);
            newNodeLoaded = true;
        }
        
        var loadedGpxTours = repositoryNode
            .GetAssociatedToursDeep()
            .ToArray();
        
        await srvRecentlyOpened.AddOpenedAsync(
            fileToOpen, RecentlyOpenedType.File);

        if (newNodeLoaded)
        {
            srvMessagePublisher.Publish(new GpxFileRepositoryNodesLoadedMessage([repositoryNode]));
        }
        srvMessagePublisher.Publish(new GpxToursSelectedMessage(loadedGpxTours));
        srvMessagePublisher.Publish(new ZoomToGpxToursRequestMessage(loadedGpxTours));
    }
}