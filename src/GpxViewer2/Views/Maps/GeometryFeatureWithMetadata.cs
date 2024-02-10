using GpxViewer2.Model;
using Mapsui.Nts;

namespace GpxViewer2.Views.Maps;

public class GeometryFeatureWithMetadata : GeometryFeature
{
    public LoadedGpxFile? Route { get; set; }
}