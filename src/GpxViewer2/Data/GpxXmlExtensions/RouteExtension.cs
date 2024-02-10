using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GpxViewer2.Data.GpxXmlExtensions;

[XmlType("RouteExtension", Namespace = "http://gpxviewer.rolandk.net/")]
public class RouteExtension : TourExtension
{
}