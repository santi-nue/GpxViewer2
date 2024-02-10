using Avalonia.Controls;
using Mapsui.Layers;
using Mapsui.Tiling;

namespace GpxViewer2.Views;

public partial class MapView : UserControl
{
    private MemoryLayer _lineStringLayerForAll;
    private MemoryLayer _lineStringLayerForSelection;
    
    public MapView()
    {
        this.InitializeComponent();
        
        _lineStringLayerForAll = new MemoryLayer();
        _lineStringLayerForAll.IsMapInfoLayer = true;
        _lineStringLayerForSelection = new MemoryLayer();
        
        this.MapControl.Map!.Layers.Add(OpenStreetMap.CreateTileLayer());
        this.MapControl.Map.Layers.Add(_lineStringLayerForAll);
        this.MapControl.Map.Layers.Add(_lineStringLayerForSelection);
        this.MapControl.UnSnapRotationDegrees = 30;
        this.MapControl.ReSnapRotationDegrees = 5;
    }
}