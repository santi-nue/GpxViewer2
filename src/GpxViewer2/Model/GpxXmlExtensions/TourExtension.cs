using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxViewer2.Model.GpxXmlExtensions;

public abstract class TourExtension
{
    public GpxTrackState State { get; set; } = GpxTrackState.Unknown;
}