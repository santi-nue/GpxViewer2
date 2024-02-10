using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RolandK.Formats.Gpx;

namespace GpxViewer2.Data;

internal class LoadedGpxFileWaypointInfo
{
    public LoadedGpxFile File { get; }
        
    public GpxWaypoint RawWaypointData { get; }

    public LoadedGpxFileWaypointInfo(LoadedGpxFile gpxFile, GpxWaypoint rawWaypoint)
    {
        this.File = gpxFile;
        this.RawWaypointData = rawWaypoint;
    }
}