using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using GpxViewer2.Messages;
using GpxViewer2.Services.GpxFileStore;
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

    private IRouteSelectionViewService? _routeSelectionViewService;

    /// <inheritdoc />
    public string Title { get; } = "Routes";

    public ObservableCollection<RouteSelectionNode> Nodes { get; } = new();
    
    /// <inheritdoc />
    public Control CreateViewInstance()
    {
        return new RouteSelectionView();
    }

    public void NotifyDoubleTabbed(RouteSelectionNode node)
    {
        using var scope = this.GetScopedService(out SelectGpxToursUseCase useCase);
        useCase.SelectGpxTours(node.Node
            .GetAssociatedToursDeep()
            .ToArray());
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

    [RelayCommand]
    private void CloseNodes()
    {
        if (_routeSelectionViewService == null) { return; }
        
        var selectedNodes = _routeSelectionViewService.GetSelectedNodes();
        if (selectedNodes.Count == 0) { return; }

        var nodesToRemove = selectedNodes
            .Select(x => x.Node)
            .ToArray();

        using var scope = base.GetScopedService(out CloseFileOrDirectoryUseCase useCase);
        useCase.CloseFileOrDirectories(nodesToRemove);
    }

    private void OnRouteSelectionViewService_NodeSelectionChanged(object? sender, EventArgs e)
    {
        
    }
    
    private void OnMessageReceived(GpxFileRepositoryNodesLoadedMessage message)
    {
        foreach (var actLoadedNode in message.Nodes)
        {
            this.Nodes.Add(new RouteSelectionNode(actLoadedNode));
        }
    }

    private void OnMessageReceived(GpxFileRepositoryNodesRemovedMessage message)
    {
        RemoveNodesDeep(message.Nodes, this.Nodes);
    }

    private static void RemoveNodesDeep(
        IReadOnlyList<GpxFileRepositoryNode> removedRepoNodes,
        ObservableCollection<RouteSelectionNode> treeNodes)
    {
        var removedNodes = treeNodes
            .Where(x => removedRepoNodes.Contains(x.Node))
            .ToArray();
        foreach (var actRemovedNode in removedNodes)
        {
            treeNodes.Remove(actRemovedNode);
        }

        foreach (var actRemainingTreeNode in treeNodes)
        {
            if(actRemainingTreeNode.ChildNodes.Count == 0){ continue; }
            RemoveNodesDeep(removedRepoNodes, actRemainingTreeNode.ChildNodes);
        }
    }

    /// <inheritdoc />
    protected override void OnAssociatedViewChanged(object? associatedView)
    {
        base.OnAssociatedViewChanged(associatedView);

        if (_routeSelectionViewService != null)
        {
            _routeSelectionViewService.NodeSelectionChanged -= this.OnRouteSelectionViewService_NodeSelectionChanged;
            _routeSelectionViewService = null;
        }

        if (associatedView is IRouteSelectionViewService viewService)
        {
            _routeSelectionViewService = viewService;
            _routeSelectionViewService.NodeSelectionChanged += this.OnRouteSelectionViewService_NodeSelectionChanged;
        }
    }
}