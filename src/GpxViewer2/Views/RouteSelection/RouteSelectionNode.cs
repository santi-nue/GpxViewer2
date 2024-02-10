using System.Collections.ObjectModel;
using GpxViewer2.Services.GpxFileStore;

namespace GpxViewer2.Views.RouteSelection;

public class RouteSelectionNode
{
    public ObservableCollection<RouteSelectionNode> ChildNodes { get; } = new();

    public GpxFileRepositoryNode Node { get; }

    public RouteSelectionNode(GpxFileRepositoryNode node)
    {
        this.Node = node;

        foreach (var actChildNode in node.ChildNodes)
        {
            this.ChildNodes.Add(new RouteSelectionNode(actChildNode));
        }
    }
}