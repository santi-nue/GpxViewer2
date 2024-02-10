using Avalonia.Controls;
using GpxViewer2.Util;
using RKMediaGallery.Controls;

namespace GpxViewer2.Views;

public partial class RouteDetailViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly RouteDetailViewModel EmptyViewModel = new();

    /// <inheritdoc />
    public string Title { get; } = "Route Detail";

    /// <inheritdoc />
    public Control CreateViewInstance()
    {
        return new RouteDetailView();
    }
}