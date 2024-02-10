using Avalonia.Controls;

namespace RKMediaGallery.Controls;

public interface INavigationTarget
{
    string Title { get; }
    
    /// <summary>
    /// Creates a view instance that is responsible to display this viewmodel.
    /// </summary>
    Control CreateViewInstance();
}