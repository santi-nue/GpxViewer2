using System.ComponentModel;
using GpxViewer2.Messages;
using GpxViewer2.Model;
using GpxViewer2.Model.GpxXmlExtensions;
using RolandK.InProcessMessaging;

namespace GpxViewer2.Views.RouteDetail;

public class SelectedTourPropertiesViewModel(
    LoadedGpxFileTourInfo tour,
    IInProcessMessagePublisher messagePublisher)
{
    [Category("Metadata")]
    public string Name
    {
        get => tour.RawTrackOrRoute.Name ?? string.Empty;
        set
        {
            if (tour.RawTrackOrRoute.Name != value)
            {
                tour.RawTrackOrRoute.Name = value;
                tour.File.ContentsChanged = true;

                messagePublisher.BeginPublish(
                    new TourConfigurationChangedMessage(tour));
            }
        }
    }
    
    [Category("Metadata")]
    public string Description
    {
        get => tour.RawTrackOrRoute.Description ?? string.Empty;
        set
        {
            if (tour.RawTrackOrRoute.Description != value)
            {
                tour.RawTrackOrRoute.Description = value;
                tour.File.ContentsChanged = true;

                messagePublisher.BeginPublish(
                    new TourConfigurationChangedMessage(tour));
            }
        }
    }
    
    [Category("Metadata")]
    public GpxTrackState State
    {
        get => tour.RawTourExtensionData.State;
        set
        {
            if (tour.RawTourExtensionData.State != value)
            {
                tour.RawTourExtensionData.State = value;
                tour.File.ContentsChanged = true;

                messagePublisher.BeginPublish(
                    new TourConfigurationChangedMessage(tour));
            }
        }
    }
    
    [Category("Metrics")]
    public string DistanceKm => tour.DistanceKm.ToString("N1");
    
    [Category("Metrics")]
    public string ElevationUpMeters => tour.ElevationUpMeters.ToString("N0");
    
    [Category("Metrics")]
    public string ElevationDownMeters => tour.ElevationDownMeters.ToString("N0");
}