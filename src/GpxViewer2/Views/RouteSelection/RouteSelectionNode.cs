using System.Collections.ObjectModel;
using GpxViewer2.Model;
using GpxViewer2.Model.GpxXmlExtensions;
using GpxViewer2.Services.GpxFileStore;
using GpxViewer2.Util;

namespace GpxViewer2.Views.RouteSelection;

public class RouteSelectionNode
{
    public ObservableCollection<RouteSelectionNode> ChildNodes { get; } = new();

    public LoadedGpxFileTourInfo? AssociatedTour { get; }
    
    public GpxFileRepositoryNode Node { get; }

    public bool HasAssociatedTour => AssociatedTour != null;
    
    public bool IsTourFinishedVisible 
        => this.AssociatedTour?.RawTourExtensionData.State == GpxTrackState.Succeeded;

    public double DistanceKm
        => this.AssociatedTour?.DistanceKm ?? 0.0;

    public double ElevationUpMeters
        => this.AssociatedTour?.ElevationUpMeters ?? 0.0;

    public double ElevationDownMeters
        => this.AssociatedTour?.ElevationDownMeters ?? 0.0;

    public string TooltipText
    {
        get
        {
            using var scope = PooledStringBuilders.Current.UseStringBuilder(out var stringBuilder);

            stringBuilder.Append(this.Node.NodeText);
            if (this.AssociatedTour != null)
            {
                stringBuilder.Append(", ");
                stringBuilder.Append(this.AssociatedTour.DistanceKm.ToString("N1"));
                stringBuilder.Append(" km, ");
                stringBuilder.Append(this.AssociatedTour.ElevationUpMeters.ToString("N0"));
                stringBuilder.Append(" m up, ");
                stringBuilder.Append(this.AssociatedTour.ElevationDownMeters.ToString("N0"));
                stringBuilder.Append(" m down");
            }

            return stringBuilder.ToString();
        }
    }

    public RouteSelectionNode(GpxFileRepositoryNode node)
    {
        this.Node = node;
        this.AssociatedTour = node.GetAssociatedTour();
        
        foreach (var actChildNode in node.ChildNodes)
        {
            this.ChildNodes.Add(new RouteSelectionNode(actChildNode));
        }
    }
}