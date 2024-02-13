using System;
using System.Collections.Generic;

namespace GpxViewer2.Views.RouteSelection;

public interface IRouteSelectionViewService
{
    event EventHandler NodeSelectionChanged;
    
    IReadOnlyList<RouteSelectionNode> GetSelectedNodes();
}