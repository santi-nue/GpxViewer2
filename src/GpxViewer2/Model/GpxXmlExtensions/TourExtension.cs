namespace GpxViewer2.Model.GpxXmlExtensions;

public abstract class TourExtension
{
    public GpxTrackState State { get; set; } = GpxTrackState.Unknown;
}
