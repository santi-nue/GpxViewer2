using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Input;
using GpxViewer2.Model;
using GpxViewer2.Views.Maps;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Tiling;
using RolandK.AvaloniaExtensions.Mvvm.Controls;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace GpxViewer2.Views;

public partial class MapView : MvvmUserControl, IMapsViewService
{
    private MemoryLayer _lineStringLayerForAll;
    private MemoryLayer _lineStringLayerForSelection;
    
    private DateTimeOffset _lastPointerPressTimestamp = DateTimeOffset.MinValue;
    
    /// <inheritdoc />
    public event EventHandler<RouteClickedEventArgs>? RouteClicked;
    
    /// <inheritdoc />
    public event EventHandler<ViewServiceRequestEventArgs>? ViewServiceRequest;
    
    public MapView()
    {
        this.InitializeComponent();
        
        _lineStringLayerForAll = new MemoryLayer();
        _lineStringLayerForAll.IsMapInfoLayer = true;
        _lineStringLayerForSelection = new MemoryLayer();
        
        this.MapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
        this.MapControl.Map.Layers.Add(_lineStringLayerForSelection);
        this.MapControl.Map.Layers.Add(_lineStringLayerForAll);
        this.MapControl.UnSnapRotationDegrees = 30;
        this.MapControl.ReSnapRotationDegrees = 5;

        this.ViewServices.Add(this);
    }

    /// <inheritdoc />
    public void AddAvailableGpxTours(IEnumerable<LoadedGpxFileTourInfo> newGpxTours)
    {
        _lineStringLayerForAll.Features = _lineStringLayerForAll.Features.Concat(
            newGpxTours
                .SelectMany(x => x.Segments)
                .Select(actSegment =>
                {
                    return new GeometryFeatureWithMetadata()
                    {
                        Geometry = actSegment.Points.GpxWaypointsToMapsuiGeometry(),
                        Styles = new[] { GpxRenderingHelper.CreateLineStringStyleForTour(actSegment.Tour) },
                        Tour = actSegment.Tour
                    };
                }))
            .ToArray();
        
        this.MapControl.RefreshGraphics();
    }

    /// <inheritdoc />
    public void RemoveAvailableGpxTours(IEnumerable<LoadedGpxFileTourInfo> existingGpxTours)
    {
        _lineStringLayerForAll.Features = _lineStringLayerForAll.Features
            .Where(x =>
            {
                if (x is not GeometryFeatureWithMetadata feature) { return true; }

                return !existingGpxTours.Contains(feature.Tour);
            })
            .ToArray();
        _lineStringLayerForSelection.Features = _lineStringLayerForAll.Features
            .Where(x =>
            {
                if (x is not GeometryFeatureWithMetadata feature) { return true; }

                return !existingGpxTours.Contains(feature.Tour);
            })
            .ToArray();
        
        this.MapControl.RefreshGraphics();
    }

    /// <inheritdoc />
    public void UpdateGpxTourStyles()
    {
        foreach (var actFeature in _lineStringLayerForAll.Features)
        {
            if ((actFeature is not GeometryFeatureWithMetadata featureWithMetadata) ||
                (featureWithMetadata.Tour == null))
            {
                continue;
            }

            featureWithMetadata.Styles = new[]
            {
                GpxRenderingHelper.CreateLineStringStyleForTour(featureWithMetadata.Tour)
            };
        }
    }

    /// <inheritdoc />
    public void SetSelectedGpxTours(IReadOnlyList<LoadedGpxFileTourInfo> selection)
    {
        if (selection.Count == 0)
        {
            _lineStringLayerForSelection.Features = Array.Empty<GeometryFeature>();
        }
        else
        {
            _lineStringLayerForSelection.Features = selection
                .SelectMany(x => x.Segments)
                .Select(actSegment =>
                {
                    return new GeometryFeatureWithMetadata()
                    {
                        Geometry = actSegment.Points.GpxWaypointsToMapsuiGeometry(),
                        Styles = new[] { GpxRenderingHelper.CreateLineStringStyle(GpxTourLineStringType.Selected) },
                        Tour = actSegment.Tour
                    };
                })
                .ToArray();
        }
        
        this.MapControl.RefreshGraphics();
    }

    private void OnMapControl_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _lastPointerPressTimestamp = DateTimeOffset.UtcNow;
    }
    
    private void OnMapControl_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (DateTimeOffset.UtcNow - _lastPointerPressTimestamp > TimeSpan.FromMilliseconds(300)) { return; }

        if (e.InitialPressMouseButton == MouseButton.Right)
        {
            this.RouteClicked?.Invoke(this, new RouteClickedEventArgs(null));
            return;
        }

        if (e.InitialPressMouseButton == MouseButton.Left)
        {
            var mousePosition = e.GetCurrentPoint(this.MapControl);
        
            var clickInfo = this.MapControl.GetMapInfo(
                new MPoint(mousePosition.Position.X, mousePosition.Position.Y),
                3);
            if (clickInfo?.Feature is GeometryFeatureWithMetadata featureWithMetadata)
            {
                this.RouteClicked?.Invoke(this, new RouteClickedEventArgs(featureWithMetadata.Tour));
            }
            else
            {
                this.RouteClicked?.Invoke(this, new RouteClickedEventArgs(null));
            }
        }
    }

    private void OnMapControl_PointerMoved(object? sender, PointerEventArgs e)
    {
        var mousePosition = e.GetCurrentPoint(this.MapControl);
        
        var mouseLocationInfo = this.MapControl.GetMapInfo(
            new MPoint(mousePosition.Position.X, mousePosition.Position.Y),
            3);

        this.MapControl.Cursor = mouseLocationInfo?.Feature != null
            ? new Cursor(StandardCursorType.Hand)
            : Cursor.Default;
    }
}