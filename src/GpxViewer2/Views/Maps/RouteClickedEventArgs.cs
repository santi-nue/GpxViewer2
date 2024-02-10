using System;
using GpxViewer2.Model;

namespace GpxViewer2.Views.Maps;

public class RouteClickedEventArgs(LoadedGpxFile? clickedGpxFile) : EventArgs
{
    public LoadedGpxFile? ClickedGpxFile { get; } = clickedGpxFile;
}