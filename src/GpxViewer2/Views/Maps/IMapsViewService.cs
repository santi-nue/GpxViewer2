using System;
using System.Collections.Generic;
using GpxViewer2.Model;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace GpxViewer2.Views.Maps;

public interface IMapsViewService : IViewService
{
    event EventHandler<RouteClickedEventArgs> RouteClicked; 

    void AddAvailableGpxTours(IEnumerable<LoadedGpxFileTourInfo> newGpxTours);

    void UpdateGpxTourStyles();
    
    void SetSelectedGpxTours(IReadOnlyList<LoadedGpxFileTourInfo> selection);
}