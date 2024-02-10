using System;
using System.Collections.Generic;
using GpxViewer2.Model;
using RolandK.AvaloniaExtensions.ViewServices.Base;

namespace GpxViewer2.Views.Maps;

public interface IMapsViewService : IViewService
{
    event EventHandler<RouteClickedEventArgs> RouteClicked; 

    void AddAvailableGpxFiles(IEnumerable<LoadedGpxFile> newGpxFiles);
    
    void SetSelectedGpxFile(IReadOnlyList<LoadedGpxFile> selection);
}