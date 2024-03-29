namespace GpxViewer2.Model.GpxXmlExtensions;

public abstract class TourExtension
{
    public bool IsTopTour { get; set; }
    
    public GpxTrackState State { get; set; } = GpxTrackState.Unknown;
}
