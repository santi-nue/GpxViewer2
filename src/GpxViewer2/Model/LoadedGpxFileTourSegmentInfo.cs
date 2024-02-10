using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RolandK.Formats.Gpx;

namespace GpxViewer2.Model;

public class LoadedGpxFileTourSegmentInfo
{
    public List<GpxWaypoint> Points { get; }
        
    public LoadedGpxFileTourSegmentInfo(GpxRoute route)
    {
        this.Points = route.RoutePoints;
    }

    public LoadedGpxFileTourSegmentInfo(GpxTrackSegment trackSegment)
    {
        this.Points = trackSegment.Points;
    }
}