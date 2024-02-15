using Mapsui;

namespace GpxViewer2.Views.Maps;

internal class NavigationMRectBuilder
{
    private MRect? _rect;

    public bool CanBuildBoundingBox => _rect != null;

    public void TryAddFeature(IFeature feature)
    {
        if (feature.Extent == null) { return; }
        
        if (_rect == null) { _rect = feature.Extent; }
        else { _rect = _rect.Join(feature.Extent); }
    }

    public MRect? TryBuild()
    {
        return _rect?.Grow(
            _rect.Width * 0.1, _rect.Height * 0.1);
    }
}