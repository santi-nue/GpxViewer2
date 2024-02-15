using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using GpxViewer2.Messages;
using GpxViewer2.Model;
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
    private bool _isInSelectionProcessing;

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
        if (_isInSelectionProcessing) { return; }
        _isInSelectionProcessing = true;
        try
        {
            using var scope = this.GetScopedService(out SelectGpxToursUseCase useCase);
            useCase.SelectGpxTours(
                node.Node.GetAssociatedToursDeep().ToArray(), 
                true);
        }
        finally
        {
            _isInSelectionProcessing = false;
        }
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

    private static IEnumerable<RouteSelectionNode> GetCorrespondingNodes(
        IReadOnlyList<LoadedGpxFileTourInfo> tours,
        ObservableCollection<RouteSelectionNode> treeNodes)
    {
        foreach (var actTreeNode in treeNodes)
        {
            if (tours.Any(x => x.File == actTreeNode.Node.GetAssociatedGpxFile()))
            {
                yield return actTreeNode;
            }
            
            if(actTreeNode.ChildNodes.Count == 0){ continue; }
            foreach (var actFoundNode in GetCorrespondingNodes(tours, actTreeNode.ChildNodes))
            {
                yield return actFoundNode;
            }
        }
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

    [RelayCommand]
    private void ZoomToSelectedNodes()
    {
        if (_routeSelectionViewService == null) { return; }
        
        if (_isInSelectionProcessing) { return; }
        _isInSelectionProcessing = true;
        try
        {
            var selectedTours = _routeSelectionViewService.GetSelectedNodes()
                .SelectMany(x => x.Node.GetAssociatedToursDeep())
                .Distinct()
                .ToArray();

            using var scope = base.GetScopedService(out SelectGpxToursUseCase useCase);
            useCase.SelectGpxTours(selectedTours, true);
        }
        finally
        {
            _isInSelectionProcessing = false;
        }
    }

    private void OnRouteSelectionViewService_NodeSelectionChanged(object? sender, EventArgs e)
    {
        if (_routeSelectionViewService == null) { return; }
        
        if (_isInSelectionProcessing) { return; }
        _isInSelectionProcessing = true;
        try
        {
            var selectedNodes = _routeSelectionViewService.GetSelectedNodes();

            var toursToSelect = selectedNodes
                .SelectMany(x => x.Node.GetAssociatedToursDeep())
                .Distinct()
                .ToArray();

            using var scope = base.GetScopedService(out SelectGpxToursUseCase useCase);
            useCase.SelectGpxTours(toursToSelect, false);
        }
        finally
        {
            _isInSelectionProcessing = false;
        }
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

    private void OnMessageReceived(GpxToursSelectedMessage message)
    {
        if (_routeSelectionViewService == null) { return; }
        
        if (_isInSelectionProcessing) { return; }
        _isInSelectionProcessing = true;
        try
        {
            var correspondingNodes = 
                GetCorrespondingNodes(message.GpxTours, this.Nodes)
                .ToArray();
            _routeSelectionViewService.SetSelectedNodes(correspondingNodes);
        }
        finally
        {
            _isInSelectionProcessing = false;
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