using Avalonia.Controls;
using GpxViewer2.Util;
using RKMediaGallery.Controls;

namespace GpxViewer2.Views;

public partial class MapViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly MapViewModel EmptyViewModel = new();

    /// <inheritdoc />
    public string Title { get; } = "Map";

    /// <inheritdoc />
    public Control CreateViewInstance()
    {
        return new MapView();
    }
}