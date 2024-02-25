using System.ComponentModel;
using GpxViewer2.Model;
using GpxViewer2.Model.GpxXmlExtensions;
using GpxViewer2.UseCases;

namespace GpxViewer2.Views.RouteDetail;

public class SelectedTourPropertiesViewModel(
    LoadedGpxFileTourInfo tour,
    UpdateTourPropertyUseCase useCase)
{
    [Category("Metadata")]
    public string Name
    {
        get => tour.RawTrackOrRoute.Name ?? string.Empty;
        set => useCase.SetTourName(tour, value);
    }
    
    [Category("Metadata")]
    public string Description
    {
        get => tour.RawTrackOrRoute.Description ?? string.Empty;
        set => useCase.SetTourDescription(tour, value);
    }
    
    [Category("Metadata")]
    public GpxTrackState State
    {
        get => tour.RawTourExtensionData.State;
        set => useCase.SetTourState(tour, value);
    }
    
    [Category("Metrics")]
    public string DistanceKm => tour.DistanceKm.ToString("N1");
    
    [Category("Metrics")]
    public string ElevationUpMeters => tour.ElevationUpMeters.ToString("N0");
    
    [Category("Metrics")]
    public string ElevationDownMeters => tour.ElevationDownMeters.ToString("N0");
}