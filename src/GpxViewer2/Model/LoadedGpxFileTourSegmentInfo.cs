using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RolandK.Formats.Gpx;

namespace GpxViewer2.Model;

public class LoadedGpxFileTourSegmentInfo
{
    public LoadedGpxFileTourInfo Tour { get; }
    
    public List<GpxWaypoint> Points { get; }
        
    public LoadedGpxFileTourSegmentInfo(
        LoadedGpxFileTourInfo tour,
        GpxRoute route)
    {
        this.Tour = tour;
        this.Points = route.RoutePoints;
    }

    public LoadedGpxFileTourSegmentInfo(
        LoadedGpxFileTourInfo tour,
        GpxTrackSegment trackSegment)
    {
        this.Tour = tour;
        this.Points = trackSegment.Points;
    }
}