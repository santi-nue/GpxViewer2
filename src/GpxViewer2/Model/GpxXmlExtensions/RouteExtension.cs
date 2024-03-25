using System.Xml.Serialization;

namespace GpxViewer2.Model.GpxXmlExtensions;

[XmlType("RouteExtension", Namespace = "http://gpxviewer.rolandk.net/")]
public class RouteExtension : TourExtension
{
}
