using GpxViewer2.Model;
using Mapsui.Nts;

namespace GpxViewer2.Views.Maps;

public class GeometryFeatureWithMetadata : GeometryFeature
{
    public LoadedGpxFileTourInfo? Tour { get; set; }
}