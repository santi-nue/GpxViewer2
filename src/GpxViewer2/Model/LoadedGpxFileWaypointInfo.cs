using RolandK.Formats.Gpx;

namespace GpxViewer2.Model;

public class LoadedGpxFileWaypointInfo
{
    public LoadedGpxFile File { get; }

    public GpxWaypoint RawWaypointData { get; }

    public LoadedGpxFileWaypointInfo(LoadedGpxFile gpxFile, GpxWaypoint rawWaypoint)
    {
        this.File = gpxFile;
        this.RawWaypointData = rawWaypoint;
    }
}
