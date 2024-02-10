using Avalonia.Controls;
using GpxViewer2.Util;
using RKMediaGallery.Controls;

namespace GpxViewer2.Views;

public partial class RouteSelectionViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly RouteSelectionViewModel EmptyViewModel = new();

    /// <inheritdoc />
    public string Title { get; } = "Routes";

    /// <inheritdoc />
    public Control CreateViewInstance()
    {
        return new RouteSelectionView();
    }
}