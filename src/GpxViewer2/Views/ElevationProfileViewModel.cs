using Avalonia.Controls;
using GpxViewer2.Util;
using RKMediaGallery.Controls;

namespace GpxViewer2.Views;

public partial class ElevationProfileViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly ElevationProfileViewModel EmptyViewModel = new();
    
    /// <inheritdoc />
    public string Title { get; } = "Navigation profile";

    /// <inheritdoc />
    public Control CreateViewInstance()
    {
        return new ElevationProfileView();
    }
}