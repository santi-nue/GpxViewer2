using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using GpxViewer2.Messages;
using GpxViewer2.Model;
using GpxViewer2.Util;
using GpxViewer2.Views.RouteDetail;
using RKMediaGallery.Controls;
using RolandK.InProcessMessaging;

namespace GpxViewer2.Views;

public partial class RouteDetailViewModel : OwnViewModelBase, INavigationTarget
{
    public static readonly RouteDetailViewModel EmptyViewModel = new();

    [ObservableProperty]
    private string _selectedRouteDescription = string.Empty;

    [ObservableProperty]
    private bool _showRouteDetail = false;

    [ObservableProperty]
    private SelectedTourPropertiesViewModel? _selectedTour = null;
    
    /// <inheritdoc />
    public string Title { get; } = "Selected Tour";

    /// <inheritdoc />
    public Control CreateViewInstance()
    {
        return new RouteDetailView();
    }

    private void OnMessageReceived(GpxToursSelectedMessage message)
    {
        if (message.GpxTours.Count == 1)
        {
            base.GetService(out IInProcessMessagePublisher messagePublisher);
            this.SelectedTour = new SelectedTourPropertiesViewModel(
                message.GpxTours[0],
                messagePublisher);
        }
        else
        {
            this.SelectedTour = null;
        }
        
        this.SelectedRouteDescription = message.GpxTours.Count switch
        {
            0 => "None",
            1 => message.GpxTours[0].File.FileName,
            _ => "More files"
        };
        this.ShowRouteDetail = message.GpxTours.Count switch
        {
            0 => false,
            1 => true,
            _ => false
        };
    }
}