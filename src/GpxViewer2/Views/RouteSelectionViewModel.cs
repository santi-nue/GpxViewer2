using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using GpxViewer2.Messages;
using GpxViewer2.UseCases;
using GpxViewer2.Util;
using GpxViewer2.Views.RouteSelection;
using RKMediaGallery.Controls;
using RolandK.AvaloniaExtensions.ViewServices;
using FileDialogFilter = RolandK.AvaloniaExtensions.ViewServices.FileDialogFilter;

namespace GpxViewer2.Views;

public partial class RouteSelectionViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly RouteSelectionViewModel EmptyViewModel = new();

    /// <inheritdoc />
    public string Title { get; } = "Routes";

    public ObservableCollection<RouteSelectionNode> Nodes { get; } = new();
    
    /// <inheritdoc />
    public Control CreateViewInstance()
    {
        return new RouteSelectionView();
    }
    
    [RelayCommand]
    private async Task LoadGpxFileAsync()
    {
        var srvOpenFileDialog = this.GetViewService<IOpenFileViewService>();
        var fileToOpen = await srvOpenFileDialog.ShowOpenFileDialogAsync(
            [new FileDialogFilter("GPX-Files (*.gpx)", ["*.gpx"])],
            "Load GPX-File");
        if (string.IsNullOrEmpty(fileToOpen)) { return; }

        using var scope = this.GetScopedService(out LoadGpxFileUseCase useCase);
        useCase.LoadGpxFile(fileToOpen);
    }

    [RelayCommand]
    private async Task LoadGpxDirectoryAsync()
    {
        var srvOpenDirectoryDialog = this.GetViewService<IOpenDirectoryViewService>();
        var directoryToOpen = await srvOpenDirectoryDialog.ShowOpenDirectoryDialogAsync(
            "Load Directory");
        if (string.IsNullOrEmpty(directoryToOpen)) { return; }

        using var scope = this.GetScopedService(out LoadGpxDirectoryUseCase useCase);
        useCase.LoadGpxDirectory(directoryToOpen);
    }

    private void OnMessageReceived(GpxFileRepositoryNodesLoadedMessage message)
    {
        foreach (var actLoadedNode in message.Nodes)
        {
            this.Nodes.Add(new RouteSelectionNode(actLoadedNode));
        }
    }

    private void OnMessageReceived(GpxToursSelectedMessage message)
    {
        
    }
}