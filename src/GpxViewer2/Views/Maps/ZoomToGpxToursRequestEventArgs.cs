using System;
using System.Collections.Generic;
using GpxViewer2.Model;

namespace GpxViewer2.Views.Maps;

public class ZoomToGpxToursRequestEventArgs(
    IReadOnlyList<LoadedGpxFileTourInfo> tours) : EventArgs
{
    public IReadOnlyList<LoadedGpxFileTourInfo> Tours => tours;
}