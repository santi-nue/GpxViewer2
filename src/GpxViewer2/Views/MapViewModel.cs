using System;
using Avalonia.Controls;
using GpxViewer2.Messages;
using GpxViewer2.UseCases;
using GpxViewer2.Util;
using GpxViewer2.Views.Maps;
using RKMediaGallery.Controls;

namespace GpxViewer2.Views;

public partial class MapViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly MapViewModel EmptyViewModel = new();
    
    private IMapsViewService? _attachedMapsViewService;
    
    /// <inheritdoc />
    public string Title { get; } = "Map";

    public event EventHandler<ZoomToGpxToursRequestEventArgs>? ZoomToGpxToursRequest; 

    /// <inheritdoc />
    public Control CreateViewInstance()
    {
        return new MapView();
    }
    
    private async void OnAttachedMapsViewService_RouteClicked(object? sender, RouteClickedEventArgs e)
    {
        this.WrapWithErrorHandling(() =>
        {
            using var scope = this.GetScopedService(out SelectGpxToursUseCase useCase);

            if (e.ClickedGpxTour == null)
            {
                useCase.SelectGpxTours([], false);
                return;
            }

            useCase.SelectGpxTours([e.ClickedGpxTour], false);
        });
    }
    
    private void OnAttachedMapsViewService_RouteDoubleClicked(object? sender, RouteClickedEventArgs e)
    {
        if (e.ClickedGpxTour == null) { return; }
        
        this.WrapWithErrorHandling(() =>
        {
            using var scope = this.GetScopedService(out SelectGpxToursUseCase useCase);
            useCase.SelectGpxTours(
                [e.ClickedGpxTour],
                true);
        });
    }
    
    public void OnMessageReceived(GpxFileRepositoryNodesLoadedMessage message)
    {
        var srvMaps = this.GetViewService<IMapsViewService>();

        foreach (var actNode in message.Nodes)
        {
            srvMaps.AddAvailableGpxTours(actNode.GetAssociatedToursDeep());
        }
    }
    
    public void OnMessageReceived(GpxToursSelectedMessage message)
    {
        var srvMaps = this.GetViewService<IMapsViewService>();
        srvMaps.SetSelectedGpxTours(message.GpxTours);
    }

    public void OnMessageReceived(TourConfigurationChangedMessage message)
    {
        var srvMaps = this.GetViewService<IMapsViewService>();
        srvMaps.UpdateGpxTourVisualization();
    }

    public void OnMessageReceived(GpxFileRepositoryNodesRemovedMessage message)
    {
        var srvMaps = this.GetViewService<IMapsViewService>();

        foreach (var actNode in message.Nodes)
        {
            srvMaps.RemoveAvailableGpxTours(actNode.GetAssociatedToursDeep());
        }
    }

    public void OnMessageReceived(ZoomToGpxToursRequestMessage message)
    {
        this.ZoomToGpxToursRequest?.Invoke(
            this, 
            new ZoomToGpxToursRequestEventArgs(message.Tours));
    }

    /// <inheritdoc />
    protected override void OnAssociatedViewChanged(object? associatedView)
    {
        base.OnAssociatedViewChanged(associatedView);

        if (_attachedMapsViewService != null)
        {
            _attachedMapsViewService.RouteClicked -= OnAttachedMapsViewService_RouteClicked;
            _attachedMapsViewService.RouteDoubleClicked -= OnAttachedMapsViewService_RouteDoubleClicked;
            _attachedMapsViewService = null;
        }
        
        if (associatedView is IMapsViewService attachedMapsViewService)
        {
            _attachedMapsViewService = attachedMapsViewService;
            _attachedMapsViewService.RouteClicked += OnAttachedMapsViewService_RouteClicked;
            _attachedMapsViewService.RouteDoubleClicked += OnAttachedMapsViewService_RouteDoubleClicked;
        }
    }
}