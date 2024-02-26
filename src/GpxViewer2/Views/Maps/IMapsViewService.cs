using System;
using System.Collections.Generic;
using GpxViewer2.Model;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace GpxViewer2.Views.Maps;

public interface IMapsViewService : IViewService
{
    event EventHandler<RouteClickedEventArgs> RouteClicked; 

    event EventHandler<RouteClickedEventArgs> RouteDoubleClicked; 
    
    void AddAvailableGpxTours(IEnumerable<LoadedGpxFileTourInfo> newGpxTours);

    void RemoveAvailableGpxTours(IEnumerable<LoadedGpxFileTourInfo> existingGpxTours);

    void UpdateGpxTourVisualization();
    
    void SetSelectedGpxTours(IReadOnlyList<LoadedGpxFileTourInfo> selection);
}